using ChessaSystem.Data;      // Importando o DbContext
using ChessaSystem.Models;    // Importando os modelos necessários
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ChessaSystem.Data
{
    public class FuncionarioRepository
    {
        private readonly AppDbContext _context;

        public FuncionarioRepository(AppDbContext context)
        {
            _context = context;
        }

        // Método para adicionar um novo funcionário
        public async Task AddFuncionarioAsync(Funcionario funcionario)
        {
            await _context.Funcionario.AddAsync(funcionario);
            await _context.SaveChangesAsync();
        }

        // Método para buscar todos os funcionários
        public async Task<List<Funcionario>> GetAllFuncionariosAsync()
        {
            return await _context.Funcionario.ToListAsync();
        }

        // Método para buscar um funcionário específico por ID
        public async Task<Funcionario> GetFuncionarioByIdAsync(int id)
        {
            return await _context.Funcionario
                .FirstOrDefaultAsync(f => f.FuncionarioId == id);
        }

        // Método para atualizar os dados de um funcionário
        public async Task UpdateFuncionarioAsync(Funcionario funcionario)
        {
            _context.Funcionario.Update(funcionario);
            await _context.SaveChangesAsync();
        }

        // Método para excluir um funcionário
        public async Task DeleteFuncionarioAsync(int id)
        {
            var funcionario = await _context.Funcionario
                .FirstOrDefaultAsync(f => f.FuncionarioId == id);

            if (funcionario != null)
            {
                _context.Funcionario.Remove(funcionario);
                await _context.SaveChangesAsync();
            }
        }
    }
}