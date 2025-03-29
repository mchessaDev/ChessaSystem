using ChessaSystem.Data;
using ChessaSystem.Models.Departamentos; // Certifique-se que o modelo está neste namespace
using ChessaSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ChessaSystem.Services;

namespace ChessaSystem.Controllers.Departamentos
{
    public class DepartamentoController : Controller
    {
        private readonly DepartamentoRepository _departamentoRepository;

        public DepartamentoController(DepartamentoRepository departamentoRepository)
        {
            _departamentoRepository = departamentoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var departamentos = await _departamentoRepository.GetAllDepartamentosAsync();
            return View(departamentos);
        }

        [HttpGet]
        public IActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Novo(DepartamentoViewModel departamentoViewModel)
        {
            if (ModelState.IsValid)
            {
                // Use o namespace completo para evitar conflitos
                var departamento = new ChessaSystem.Models.Departamentos.Departamento
                {
                    Nome = departamentoViewModel.Nome
                };

                await _departamentoRepository.AddDepartamentoAsync(departamento);
                return RedirectToAction(nameof(Listar));
            }

            return View(departamentoViewModel);
        }
    }
}