using ChessaSystem.Data; // Para o AppDbContext
using ChessaSystem.Models.Cargos; // Importar o namespace correto para o modelo Cargo
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChessaSystem.Services // Este é o namespace correto para um repositório
{
    public class CargoRepository
    {
        private readonly AppDbContext _context;

        public CargoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cargo>> GetAllCargosAsync()
        {
            return await _context.Cargo.ToListAsync();
        }

        public async Task<Cargo> GetCargoByIdAsync(int id)
        {
            return await _context.Cargo
                .FirstOrDefaultAsync(c => c.CargoId == id);
        }

        public async Task AddCargoAsync(Cargo cargo)
        {
            _context.Cargo.Add(cargo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCargoAsync(Cargo cargo)
        {
            _context.Cargo.Update(cargo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCargoAsync(int id)
        {
            var cargo = await _context.Cargo
                .FirstOrDefaultAsync(c => c.CargoId == id);

            if (cargo != null)
            {
                _context.Cargo.Remove(cargo);
                await _context.SaveChangesAsync();
            }
        }
    }
}