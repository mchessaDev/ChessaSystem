using ChessaSystem.Data;
using ChessaSystem.Models.Estados; // Certifique-se que o modelo Estado está neste namespace
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChessaSystem.Data.Repositories
{
    public class EstadoRepository
    {
        private readonly AppDbContext _context;

        public EstadoRepository(AppDbContext context)
        {
            _context = context;
        }

        // Método que estava faltando
        public async Task<List<Estado>> GetAllEstadosAsync()
        {
            return await _context.Estados.ToListAsync();
        }

        public async Task<Estado> GetEstadoByIdAsync(int id)
        {
            return await _context.Estados.FirstOrDefaultAsync(e => e.EstadoId == id);
        }

        public async Task AddEstadoAsync(Estado estado)
        {
            _context.Estados.Add(estado);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateEstadoAsync(Estado estado)
        {
            _context.Estados.Update(estado);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEstadoAsync(int id)
        {
            var estado = await _context.Estados.FirstOrDefaultAsync(e => e.EstadoId == id);

            if (estado != null)
            {
                _context.Estados.Remove(estado);
                await _context.SaveChangesAsync();
            }
        }
    }
}