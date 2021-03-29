using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Odonto.Data;
using Odonto.Models;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace Odonto.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var marcacoes = _context.Marcacoes
                .Include(c => c.Procedimento)
                .Include(c => c.Agenda)
                .ThenInclude(x => x.Profissional)
                .OrderByDescending(x => x.DataDeInicio)
                .Where(t => t.UsuarioId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                .ToList();
            
            return View(marcacoes);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
