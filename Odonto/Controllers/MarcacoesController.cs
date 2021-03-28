using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Odonto.Data;
using Odonto.Models;

namespace Odonto.Controllers
{
    [Authorize]
    public class MarcacoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MarcacoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Marcacoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Marcacoes.Include(x => x.Procedimento).ToListAsync());
        }

        // GET: Marcacoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marcacao = await _context.Marcacoes
                .Include(x => x.Procedimento)
                .Include(x => x.Agenda)
                    .ThenInclude(x => x.Profissional)
                .FirstOrDefaultAsync(m => m.MarcacaoId == id);

            if (marcacao == null)
            {
                return NotFound();
            }

            return View(marcacao);
        }

        // GET: Marcacoes/Create
        public IActionResult Create()
        {
            ViewBagProcedimentos();
            return View();
        }

        // POST: Marcacoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MarcacaoId,AgendaId,ProcedimentoId,DataDeInicio,DataDeFim")] Marcacao marcacao)
        {
            if (ModelState.IsValid)
            {
                marcacao.UsuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
               
                marcacao.Marcado = true;
                _context.Add(marcacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBagProcedimentos();
            return View(marcacao);
        }

        // GET: Marcacoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marcacao = await _context.Marcacoes.FindAsync(id);
            if (marcacao == null)
            {
                return NotFound();
            }

            ViewBagProcedimentos();
            return View(marcacao);
        }

        // POST: Marcacoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MarcacaoId,AgendaId,ProcedimentoId,DataDeInicio,DataDeFim")] Marcacao marcacao)
        {
            if (id != marcacao.MarcacaoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(marcacao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MarcacaoExists(marcacao.MarcacaoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBagProcedimentos();
            return View(marcacao);
        }

        // GET: Marcacoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var marcacao = await _context.Marcacoes
                .FirstOrDefaultAsync(m => m.MarcacaoId == id);
            if (marcacao == null)
            {
                return NotFound();
            }

            return View(marcacao);
        }

        // POST: Marcacoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var marcacao = await _context.Marcacoes.FindAsync(id);
            _context.Marcacoes.Remove(marcacao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MarcacaoExists(int id)
        {
            return _context.Marcacoes.Any(e => e.MarcacaoId == id);
        }

        private void ViewBagProcedimentos()
        {
            ViewBag.Procedimentos = _context.Procedimentos.OrderBy(x => x.Nome).Select(p => new SelectListItem { Value = p.ProcedimentoId.ToString(), Text = p.Nome }).ToList();
        }
    }
}
