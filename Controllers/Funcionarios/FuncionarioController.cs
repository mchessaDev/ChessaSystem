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

        /* Acao Listar */
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

        /* Inicio do metodo Post direto via botão -*-* -*-* -*-* -*-* -*-* -*-* -*-* -*-* -*-* */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InserirDadosTeste()
        {
            try
            {
                // Criando um novo FuncionarioViewModel com dados de teste
                var funcionarioTeste = new FuncionarioViewModel
                {
                    IdQuemCriou = 1, // Pode ser um id de usuário logado
                    Matricula = "M" + new Random().Next(1000, 9999).ToString(),
                    NumeroCracha = new Random().Next(10000000, 99999999).ToString(),
                    Nome = "Teste",
                    Sobrenome = "Automático",
                    DataNascimento = DateTime.Parse("1992-04-17"),
                    Telefone = "91984-3004",
                    Rua = "Avenida Campinas, 222",
                    Bairro = "Vila Floresta",
                    CidadeId = 3,
                    EstadoId = 2,
                    Numero = "0123",
                    Complemento = "Apto 909",
                    CPF =
                        $"{new Random().Next(100, 999)}.{new Random().Next(100, 999)}.{new Random().Next(100, 999)}-{new Random().Next(10, 99)}",
                    Email = "teste" + new Random().Next(1000, 9999) + "@email.com",
                    Senha = "senhaTeste",
                    PrioridadeAcesso = 2,
                    CargoId = 1, // ID do Cargo
                    DepartamentoId = 2, // ID do Departamento
                    DataAdmissao = DateTime.Parse("2023-11-05"),
                    Ativo = true,
                    MunicipioId = 2 // ID do Município
                };

                // Convertendo o FuncionarioViewModel para o Funcionario (Modelo do banco de dados)
                var funcionario = new ChessaSystem.Models.Funcionario
                {
                    IdQuemCriou = funcionarioTeste.IdQuemCriou,
                    Matricula = funcionarioTeste.Matricula,
                    NumeroCracha = funcionarioTeste.NumeroCracha,
                    Nome = funcionarioTeste.Nome,
                    Sobrenome = funcionarioTeste.Sobrenome,
                    DataNascimento = funcionarioTeste.DataNascimento,
                    Telefone = funcionarioTeste.Telefone,
                    Rua = funcionarioTeste.Rua,
                    Bairro = funcionarioTeste.Bairro,
                    CidadeId = funcionarioTeste.CidadeId,
                    EstadoId = funcionarioTeste.EstadoId,
                    Numero = funcionarioTeste.Numero,
                    Complemento = funcionarioTeste.Complemento,
                    CPF = funcionarioTeste.CPF,
                    Email = funcionarioTeste.Email,
                    Senha = funcionarioTeste.Senha,
                    PrioridadeAcesso = funcionarioTeste.PrioridadeAcesso,
                    CargoId = funcionarioTeste.CargoId,
                    DepartamentoId = funcionarioTeste.DepartamentoId,
                    DataAdmissao = funcionarioTeste.DataAdmissao,
                    Ativo = funcionarioTeste.Ativo,
                    MunicipioId = funcionarioTeste.MunicipioId
                };

                // Adicionando o novo Funcionario no banco de dados
                _context.Funcionario.Add(funcionario);
                _context.SaveChanges();

                TempData["MensagemSucesso"] = "Inserção direta realizada com sucesso!";
                return RedirectToAction(nameof(Listar)); // Redireciona para a listagem de funcionários
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["MensagemErro"] = "Erro na inserção direta.";
                return RedirectToAction(nameof(Listar)); // Redireciona para a página de cadastro de novo funcionário
            }
        }
        /* Fim do metodo Post direto via botão -*-* -*-* -*-* -*-* -*-* -*-* -*-* -*-* -*-* */

        /* Inicio do metodo Post  via Post -*-* -*-* -*-* -*-* -*-* -*-* -*-* -*-* -*-* */

        [HttpGet]
        public IActionResult Novo()
        {
            // Preenchendo os campos de seleção
            ViewBag.Cargos = new SelectList(_context.Cargo, "CargoId", "Nome");
            ViewBag.Departamentos = new SelectList(_context.Departamento, "DepartamentoId", "Nome");
            ViewBag.Municipios = new SelectList(_context.Municipio, "MunicipioId", "Nome");
            ViewBag.Estados = new SelectList(_context.Estado, "EstadoId", "Nome");
            ViewBag.Cidades = new SelectList(_context.Cidade, "CidadeId", "Nome"); // 🔹 Verifique se está correto

            return View();
        }

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Novo(FuncionarioViewModel funcionarioTeste)
{
    try
    {
        // Gerar automaticamente Matrícula e Número do Crachá
        funcionarioTeste.Matricula ??= "M" + new Random().Next(1000, 9999).ToString();
        funcionarioTeste.NumeroCracha ??= new Random().Next(10000000, 99999999).ToString();

        // Garantir que campos FK estão preenchidos para evitar erro
        funcionarioTeste.CargoId ??= 1;
        funcionarioTeste.DepartamentoId ??= 1;
        funcionarioTeste.CidadeId ??= 3; // Ajustado conforme sua tabela de exemplo
        funcionarioTeste.EstadoId ??= 2; // Ajustado conforme sua tabela de exemplo
        funcionarioTeste.MunicipioId ??= 2;

        // Dados obrigatórios que podem causar erro caso falte
        funcionarioTeste.Nome ??= "Nome Padrão";
        funcionarioTeste.Sobrenome ??= "Sobrenome Padrão";
        funcionarioTeste.CPF ??= 
            $"{new Random().Next(100, 999)}.{new Random().Next(100, 999)}.{new Random().Next(100, 999)}-{new Random().Next(10, 99)}";
        funcionarioTeste.Telefone ??= "11999999999";
        funcionarioTeste.Email ??= "teste" + new Random().Next(1000, 9999) + "@email.com";
        funcionarioTeste.Senha ??= "senhaTeste";
        funcionarioTeste.DataAdmissao ??= DateTime.Now;

        // Verificando se a matrícula, CPF ou Email já existem
        if (_context.Funcionario.Any(f => f.Matricula == funcionarioTeste.Matricula))
        {
            ModelState.AddModelError("Matricula", "A matrícula informada já está cadastrada.");
        }

        if (_context.Funcionario.Any(f => f.CPF == funcionarioTeste.CPF))
        {
            ModelState.AddModelError("CPF", "O CPF informado já está cadastrado.");
        }

        if (_context.Funcionario.Any(f => f.Email == funcionarioTeste.Email))
        {
            ModelState.AddModelError("Email", "O email informado já está cadastrado.");
        }

        // Se houver erros, retorna à view com as mensagens de erro
        if (!ModelState.IsValid)
        {
            // Carregar os ViewBag novamente (para os selectlists)
            ViewBag.Cargos = new SelectList(_context.Cargo, "CargoId", "Nome");
            ViewBag.Departamentos = new SelectList(_context.Departamento, "DepartamentoId", "Nome");
            ViewBag.Municipios = new SelectList(_context.Municipio, "MunicipioId", "Nome");
            ViewBag.Estados = new SelectList(_context.Estado, "EstadoId", "Nome");
            ViewBag.Cidades = new SelectList(_context.Cidade, "CidadeId", "Nome");

            return View(funcionarioTeste); // Retorna com o modelo de dados atual
        }

        // Criar a entidade no banco de dados
        var funcionario = new ChessaSystem.Models.Funcionario
        {
            IdQuemCriou = funcionarioTeste.IdQuemCriou,
            Matricula = funcionarioTeste.Matricula,
            NumeroCracha = funcionarioTeste.NumeroCracha,
            Nome = funcionarioTeste.Nome,
            Sobrenome = funcionarioTeste.Sobrenome,
            DataNascimento = funcionarioTeste.DataNascimento,
            Telefone = funcionarioTeste.Telefone,
            Rua = funcionarioTeste.Rua,
            Bairro = funcionarioTeste.Bairro,
            CidadeId = funcionarioTeste.CidadeId,
            EstadoId = funcionarioTeste.EstadoId,
            Numero = funcionarioTeste.Numero,
            Complemento = funcionarioTeste.Complemento,
            CPF = funcionarioTeste.CPF,
            Email = funcionarioTeste.Email,
            Senha = funcionarioTeste.Senha,
            PrioridadeAcesso = funcionarioTeste.PrioridadeAcesso ?? 2,
            CargoId = funcionarioTeste.CargoId,
            DepartamentoId = funcionarioTeste.DepartamentoId,
            DataAdmissao = funcionarioTeste.DataAdmissao,
            Ativo = funcionarioTeste.Ativo ?? true,
            MunicipioId = funcionarioTeste.MunicipioId
        };

        // Inserção no banco de dados
        _context.Funcionario.Add(funcionario);
        _context.SaveChanges();

        TempData["MensagemSucesso"] = "Funcionário inserido com sucesso!";
        return RedirectToAction(nameof(Listar)); // Redireciona para a listagem de funcionários
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        TempData["MensagemErro"] = "Erro ao salvar no banco de dados.";
        return View(funcionarioTeste); // Retorna com o modelo atual em caso de erro
    }
}

    }
}