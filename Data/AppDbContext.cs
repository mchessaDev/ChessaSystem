using Microsoft.EntityFrameworkCore;
using ChessaSystem.Models.Departamentos;  // Certifique-se de usar o namespace correto
using ChessaSystem.Models.Estados;
using ChessaSystem.Models.Municipios;
using ChessaSystem.Models.Cargos;
using ChessaSystem.Models; // Se o modelo Funcionario estiver aqui

namespace ChessaSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Cidade> Cidade { get; set; }
        public DbSet<Departamento> Departamento { get; set; }  // Aqui o nome da tabela é singular
        public DbSet<Estado> Estado { get; set; }
        public DbSet<Municipio> Municipio { get; set; }
        public DbSet<Cargo> Cargo { get; set; }  // A tabela é singular, não 'Cargos'
        public DbSet<Funcionario> Funcionario { get; set; }  // A tabela é singular, não 'Funcionarios'

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Especificando os nomes das tabelas para garantir que o EF use os nomes corretos no banco
            modelBuilder.Entity<Departamento>().ToTable("Departamento");
            modelBuilder.Entity<Cargo>().ToTable("Cargo");
            modelBuilder.Entity<Funcionario>().ToTable("Funcionario");
        }
    }

}