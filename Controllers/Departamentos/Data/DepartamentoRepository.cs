using ChessaSystem.Models.Departamentos; // Importar o namespace correto
using ChessaSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessaSystem.Services
{
    public class DepartamentoRepository
    {
        private readonly AppDbContext _context;

        public DepartamentoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Departamento>> GetAllDepartamentosAsync()
        {
            return await _context.Departamento.ToListAsync();
        }

        public async Task<Departamento> GetDepartamentoByIdAsync(int id)
        {
            return await _context.Departamento
                .FirstOrDefaultAsync(d => d.DepartamentoId == id);
        }

        public async Task AddDepartamentoAsync(Departamento departamento)
        {
            _context.Departamento.Add(departamento);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDepartamentoAsync(Departamento departamento)
        {
            _context.Departamento.Update(departamento);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartamentoAsync(int id)
        {
            var departamento = await _context.Departamento
                .FirstOrDefaultAsync(d => d.DepartamentoId == id);

            if (departamento != null)
            {
                _context.Departamento.Remove(departamento);
                await _context.SaveChangesAsync();
            }
        }
    }
}