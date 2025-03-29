using System;
using System.ComponentModel.DataAnnotations;

namespace ChessaSystem.Models
{
    public class Cidade
    {
        public int CidadeId { get; set; }    // ID da cidade
        public string Nome { get; set; }      // Nome da cidade
    }
}