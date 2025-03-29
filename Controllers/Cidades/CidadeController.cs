using ChessaSystem.Models;
using ChessaSystem.ViewModels;
using ChessaSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChessaSystem.Controllers
{
    public class CidadeController : Controller
    {
        private readonly CidadeService _cidadeService;

        public CidadeController(CidadeService cidadeService)
        {
            _cidadeService = cidadeService;
        }

        // GET: Cidades/Listar
        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            var cidades = await _cidadeService.GetAllAsync();
            var viewModel = cidades.Select(c => new CidadeViewModel
            {
                CidadeId = c.CidadeId,
                Nome = c.Nome
            }).ToList();

            return View(viewModel);
        }

        // GET: Cidades/Novo
        [HttpGet]
        public IActionResult Novo()
        {
            return View();
        }

        // POST: Cidades/Novo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Novo(CidadeViewModel cidadeViewModel)
        {
            if (ModelState.IsValid)
            {
                var cidade = new Cidade
                {
                    Nome = cidadeViewModel.Nome
                };

                await _cidadeService.AddAsync(cidade);
                return RedirectToAction(nameof(Listar));
            }

            return View(cidadeViewModel);
        }

        // GET: Cidades/Editar/5
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var cidade = await _cidadeService.GetByIdAsync(id);
            if (cidade == null)
            {
                return NotFound();
            }

            var viewModel = new CidadeViewModel
            {
                CidadeId = cidade.CidadeId,
                Nome = cidade.Nome
            };

            return View(viewModel);
        }

        // POST: Cidades/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, CidadeViewModel cidadeViewModel)
        {
            if (id != cidadeViewModel.CidadeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var cidade = new Cidade
                {
                    CidadeId = cidadeViewModel.CidadeId,
                    Nome = cidadeViewModel.Nome
                };

                await _cidadeService.UpdateAsync(cidade);
                return RedirectToAction(nameof(Listar));
            }

            return View(cidadeViewModel);
        }

        // GET: Cidades/Excluir/5
        [HttpGet]
        public async Task<IActionResult> Excluir(int id)
        {
            var cidade = await _cidadeService.GetByIdAsync(id);
            if (cidade == null)
            {
                return NotFound();
            }

            var viewModel = new CidadeViewModel
            {
                CidadeId = cidade.CidadeId,
                Nome = cidade.Nome
            };

            return View(viewModel);
        }

        // POST: Cidades/Excluir/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarExclusao(int id)
        {
            await _cidadeService.DeleteAsync(id);
            return RedirectToAction(nameof(Listar));
        }
    }
}
