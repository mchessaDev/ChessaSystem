using ChessaSystem.Data;
using ChessaSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ChessaSystem.Data
{
    public class CidadeRepository
    {
        private readonly AppDbContext _context;

        public CidadeRepository(AppDbContext context)
        {
            _context = context;
        }

        // Método para obter todas as cidades
        public async Task<List<Cidade>> GetAllAsync()
        {
            return await _context.Cidade.ToListAsync();
        }

        // Método para obter uma cidade por ID
        public async Task<Cidade> GetByIdAsync(int id)
        {
            return await _context.Cidade
                .FirstOrDefaultAsync(c => c.CidadeId == id);
        }

        // Método para adicionar uma nova cidade
        public async Task AddAsync(Cidade cidade)
        {
            _context.Cidade.Add(cidade);
            await _context.SaveChangesAsync();
        }

        // Método para atualizar uma cidade existente
        public async Task UpdateAsync(Cidade cidade)
        {
            _context.Cidade.Update(cidade);
            await _context.SaveChangesAsync();
        }

        // Método para excluir uma cidade
        public async Task DeleteAsync(int id)
        {
            var cidade = await GetByIdAsync(id);
            if (cidade != null)
            {
                _context.Cidade.Remove(cidade);
                await _context.SaveChangesAsync();
            }
        }
    }
}