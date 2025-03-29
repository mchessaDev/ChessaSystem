using ChessaSystem.Data;
using ChessaSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace ChessaSystem.Controllers.Funcionarios
{
    public class FuncionarioController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public FuncionarioController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Listar(string searchTerm, string sortColumn, string sortOrder,
            string filtroAtivo, string filtroDepartamento, string filtroCargo, int page = 1)
        {
            List<FuncionarioViewModel> funcionarios = new List<FuncionarioViewModel>();
            int totalRegistros = 0;

            // Valores default para ordenação
            sortColumn ??= "Nome";
            sortOrder ??= "asc";

            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // 🟡 Conversões para os filtros
                object filtroAtivoBit = DBNull.Value;

                if (!string.IsNullOrEmpty(filtroAtivo))
                {
                    filtroAtivoBit = filtroAtivo == "ativo" ? 1 : 0;
                }

                object filtroDepartamentoId = string.IsNullOrEmpty(filtroDepartamento)
                    ? DBNull.Value
                    : Convert.ToInt32(filtroDepartamento);

                object filtroCargoId = string.IsNullOrEmpty(filtroCargo)
                    ? DBNull.Value
                    : Convert.ToInt32(filtroCargo);

                string baseQuery = @"
FROM dbo.Funcionario f
INNER JOIN dbo.Cargo c ON f.CargoId = c.CargoId
INNER JOIN dbo.Departamento d ON f.DepartamentoId = d.DepartamentoId
WHERE 
    (@SearchTerm IS NULL 
        OR f.Nome LIKE '%' + @SearchTerm + '%' 
        OR f.Sobrenome LIKE '%' + @SearchTerm + '%'
        OR f.Matricula LIKE '%' + @SearchTerm + '%'
        OR f.Email LIKE '%' + @SearchTerm + '%')
    AND (@FiltroAtivo IS NULL OR f.Ativo = @FiltroAtivo)
    AND (@FiltroDepartamento IS NULL OR d.DepartamentoId = @FiltroDepartamento)
    AND (@FiltroCargo IS NULL OR c.CargoId = @FiltroCargo)";

                // 🔹 Contagem de registros
                string countQuery = "SELECT COUNT(*) " + baseQuery;

                using (SqlCommand countCmd = new SqlCommand(countQuery, connection))
                {
                    countCmd.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
                    countCmd.Parameters.AddWithValue("@FiltroAtivo", filtroAtivoBit);
                    countCmd.Parameters.AddWithValue("@FiltroDepartamento", filtroDepartamentoId);
                    countCmd.Parameters.AddWithValue("@FiltroCargo", filtroCargoId);

                    connection.Open();
                    totalRegistros = (int)countCmd.ExecuteScalar();
                }

                // 🔹 Query de dados
                string query = @"
SELECT f.FuncionarioId, f.Nome, f.Sobrenome, f.Matricula, f.Email, f.PrioridadeAcesso, 
       f.Telefone, c.Nome AS Cargo, d.Nome AS Departamento, f.DataAdmissao, f.Ativo
" + baseQuery + @"
ORDER BY " + sortColumn + " " + (sortOrder == "desc" ? "DESC" : "ASC") + @"
OFFSET @Offset ROWS
FETCH NEXT @PageSize ROWS ONLY";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@SearchTerm", (object)searchTerm ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@FiltroAtivo", filtroAtivoBit);
                    cmd.Parameters.AddWithValue("@FiltroDepartamento", filtroDepartamentoId);
                    cmd.Parameters.AddWithValue("@FiltroCargo", filtroCargoId);
                    cmd.Parameters.AddWithValue("@Offset", (page - 1) * 10);
                    cmd.Parameters.AddWithValue("@PageSize", 10);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        funcionarios.Add(new FuncionarioViewModel
                        {
                            FuncionarioId = Convert.ToInt32(reader["FuncionarioId"]),
                            Nome = reader["Nome"].ToString(),
                            Sobrenome = reader["Sobrenome"].ToString(),
                            Matricula = reader["Matricula"].ToString(),
                            Email = reader["Email"].ToString(),
                            PrioridadeAcesso = reader["PrioridadeAcesso"] != DBNull.Value
                                ? Convert.ToByte(reader["PrioridadeAcesso"])
                                : (byte?)null,

                            // Aqui, recuperamos os nomes dos cargos e departamentos diretamente da consulta
                            CargoNome = reader["Cargo"].ToString(),
                            DepartamentoNome = reader["Departamento"].ToString(),

                            Telefone = reader["Telefone"] != DBNull.Value
                                ? reader["Telefone"].ToString()
                                : "N/A",

                            Ativo = reader["Ativo"] != DBNull.Value && Convert.ToBoolean(reader["Ativo"])
                        });
                    }
                }

                // 🔹 Carregar os filtros para exibir na View
                ViewBag.Departamentos = _context.Departamento
                    .Select(d => new SelectListItem
                    {
                        Value = d.DepartamentoId.ToString(),
                        Text = d.Nome
                    }).ToList();

                ViewBag.Cargos = _context.Cargo
                    .Select(c => new SelectListItem
                    {
                        Value = c.CargoId.ToString(),
                        Text = c.Nome
                    }).ToList();
            }

            // 🔹 Dados para paginação e retorno dos filtros
            ViewBag.TotalPages = (int)Math.Ceiling(totalRegistros / 10.0);
            ViewBag.CurrentPage = page;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SortColumn = sortColumn;
            ViewBag.SortOrder = sortOrder;
            ViewBag.FiltroAtivo = filtroAtivo;
            ViewBag.FiltroDepartamento = filtroDepartamento;
            ViewBag.FiltroCargo = filtroCargo;

            return View(funcionarios);
        }
    }
}
