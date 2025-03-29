using ChessaSystem.Data;
using ChessaSystem.Services;
using ChessaSystem.Models.Municipios; // Importar o namespace correto do modelo Municipio
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChessaSystem.Controllers
{
    public class MunicipioController : Controller
    {
        private readonly MunicipioRepository _municipioRepository;

        public MunicipioController(MunicipioRepository municipioRepository)
        {
            _municipioRepository = municipioRepository;
        }

        public async Task<IActionResult> Index()
        {
            var municipios = await _municipioRepository.GetAllMunicipiosAsync();
            return View(municipios);
        }

        public async Task<IActionResult> Create(ChessaSystem.Models.Municipios.Municipio municipio) // Ou apenas Municipio se o using estiver correto
        {
            if (ModelState.IsValid)
            {
                await _municipioRepository.AddMunicipioAsync(municipio);
                return RedirectToAction(nameof(Index));
            }

            return View(municipio);
        }
    }
}