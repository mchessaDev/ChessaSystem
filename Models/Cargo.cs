using System;
using System.ComponentModel.DataAnnotations;

namespace ChessaSystem.Models.Cargos
{
    public class Cargo
    {
        public int CargoId { get; set; }
        public string Nome { get; set; }
    
        public ICollection<Funcionario> Funcionarios { get; set; }
    }
}

