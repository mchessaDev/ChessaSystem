namespace ChessaSystem.ViewModels
{
    public class FuncionarioViewModel
    {
        public int FuncionarioId { get; set; } public string Nome { get; set; }
        public int IdQuemCriou { get; set; }  // O id do usuário logado
        public string NumeroCracha { get; set; }  // Número do crachá
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

        // Relacionamentos (FKs)
        public int? CargoId { get; set; }
        public int? DepartamentoId { get; set; }
        public int? EstadoId { get; set; }
        public int? CidadeId { get; set; }
        public int? MunicipioId { get; set; }

        public DateTime? DataAdmissao { get; set; }
        public DateTime? DataDemissao { get; set; }

        // ✅ Propriedades de Navegação
        public string CargoNome { get; set; }    // Nome do Cargo
        public string DepartamentoNome { get; set; }  // Nome do Departamento
    }
}