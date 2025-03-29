using ChessaSystem.Models.Cargos;
using ChessaSystem.Models.Estados;
using ChessaSystem.Models.Departamentos;
using ChessaSystem.Models.Municipios;

namespace ChessaSystem.Models
{
    public class Funcionario
    {
        public int FuncionarioId { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Matricula { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string CPF { get; set; }
        public string Senha { get; set; }
        public byte? PrioridadeAcesso { get; set; }
        public bool? Ativo { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }

        // Relacionamentos
        public int? CargoId { get; set; }
        public Cargo Cargo { get; set; }

        public int? DepartamentoId { get; set; }
        public Departamento Departamento { get; set; }

        public int? EstadoId { get; set; }
        public Estado Estado { get; set; }

        public int? MunicipioId { get; set; }
        public Municipio Municipio { get; set; }

        public DateTime? DataAdmissao { get; set; }
        public DateTime? DataDemissao { get; set; }
    }
}