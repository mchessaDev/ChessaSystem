using ChessaSystem.Data;
using ChessaSystem.Models;

namespace ChessaSystem.Services
{
    public class CidadeService
    {
        private readonly CidadeRepository _cidadeRepository;

        public CidadeService(CidadeRepository cidadeRepository)
        {
            _cidadeRepository = cidadeRepository;
        }

        public async Task<List<Cidade>> GetAllAsync()
        {
            return await _cidadeRepository.GetAllAsync();
        }

        public async Task<Cidade> GetByIdAsync(int id)
        {
            return await _cidadeRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(Cidade cidade)
        {
            await _cidadeRepository.AddAsync(cidade);
        }

        public async Task UpdateAsync(Cidade cidade)
        {
            await _cidadeRepository.UpdateAsync(cidade);
        }

        public async Task DeleteAsync(int id)
        {
            await _cidadeRepository.DeleteAsync(id);
        }
    }
}