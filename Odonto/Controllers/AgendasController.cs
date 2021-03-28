using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Odonto.Data;
using Odonto.Models;

namespace Odonto.Controllers
{
    public class AgendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AgendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Agendas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Agendas.Include(x => x.Profissional).ToListAsync());
        }

        // GET: Agendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await _context.Agendas
                .Include(x => x.Profissional)
                .FirstOrDefaultAsync(m => m.AgendaId == id);
            if (agenda == null)
            {
                return NotFound();
            }

            return View(agenda);
        }

        // GET: Agendas/Create
        public IActionResult Create()
        {
            ViewBagProfissionais();
            return View();
        }

        // POST: Agendas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AgendaId,ProfissionalId,Profissional,DataDeInicio,DataDeFim")] Agenda agenda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(agenda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBagProfissionais();
            return View(agenda);
        }

        // GET: Agendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await _context.Agendas.FindAsync(id);
            if (agenda == null)
            {
                return NotFound();
            }

            ViewBagProfissionais();
            return View(agenda);
        }

        // POST: Agendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AgendaId,ProfissionalId,Profissional,DataDeInicio,DataDeFim")] Agenda agenda)
        {
            if (id != agenda.AgendaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(agenda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AgendaExists(agenda.AgendaId))
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

            ViewBagProfissionais();
            return View(agenda);
        }

        // GET: Agendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await _context.Agendas
                .Include(x => x.Profissional)
                .FirstOrDefaultAsync(m => m.AgendaId == id);
            if (agenda == null)
            {
                return NotFound();
            }

            return View(agenda);
        }

        // POST: Agendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agenda = await _context.Agendas.FindAsync(id);
            _context.Agendas.Remove(agenda);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Agendas/ObterAgendasComDisponibilidadePorProcedimento/5
        public List<Agenda> ObterAgendasComDisponibilidadePorProcedimento(int id)
        {
            var procedimento = _context.Procedimentos.FirstOrDefault(x => x.ProcedimentoId == id);

            var agendas = _context.Agendas
                .Include(x => x.Marcacoes)
                .Include(x => x.Profissional)
                .OrderBy(x => x.DataDeInicio)
                .ToList();

            foreach (var agenda in agendas)
            {
                agenda.GerarHorariosDisponiveisPorDuracao(procedimento.DuracaoMinutos);
            }

            return agendas;
        }
        
        private bool AgendaExists(int id)
        {
            return _context.Agendas.Any(e => e.AgendaId == id);
        }

        private void ViewBagProfissionais()
        {
            ViewBag.Profissionais = _context.Profissionais.OrderBy(x => x.Nome).Select(p => new SelectListItem { Value = p.ProfissionalId.ToString(), Text = p.Nome }).ToList();
        }
    }
}
