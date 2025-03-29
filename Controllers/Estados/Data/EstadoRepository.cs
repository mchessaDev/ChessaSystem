using ChessaSystem.Models.Estados; // Novo namespace correto
using ChessaSystem.Data;

namespace ChessaSystem.Services
{
    public class EstadoRepository
    {
        private readonly AppDbContext _context;

        public EstadoRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Estado> GetAllEstados()
        {
            return _context.Estado.ToList();
        }
    }
}