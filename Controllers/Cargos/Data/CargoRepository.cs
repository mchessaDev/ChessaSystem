using ChessaSystem.Models.Municipios; // Namespace onde está o modelo Municipio
using ChessaSystem.Data; // Para o AppDbContext
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ChessaSystem.Services
{
    public class MunicipioRepository
    {
        private readonly AppDbContext _context;

        public MunicipioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Municipio>> GetAllMunicipiosAsync()
        {
            return await _context.Municipio.ToListAsync();
        }

        public async Task<Municipio> GetMunicipioByIdAsync(int id)
        {
            return await _context.Municipio.FirstOrDefaultAsync(m => m.MunicipioId == id);
        }

        public async Task AddMunicipioAsync(Municipio municipio)
        {
            _context.Municipio.Add(municipio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMunicipioAsync(Municipio municipio)
        {
            _context.Municipio.Update(municipio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMunicipioAsync(int id)
        {
            var municipio = await _context.Municipio.FirstOrDefaultAsync(m => m.MunicipioId == id);

            if (municipio != null)
            {
                _context.Municipio.Remove(municipio);
                await _context.SaveChangesAsync();
            }
        }
    }
}