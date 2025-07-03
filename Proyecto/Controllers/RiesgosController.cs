using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.Controllers
{
    public class RiesgosController : Controller
    {
        private readonly AppDbContext _context;

        public RiesgosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Riesgos
        public async Task<IActionResult> Index()
        {
            var riesgos = _context.Riesgos.Include(r => r.Activo);
            return View(await riesgos.ToListAsync());
        }

        // GET: Riesgos/Create
        public IActionResult Create()
        {
            ViewData["ActivoId"] = new SelectList(_context.Activos, "Id", "Nombre");
            return View();
        }

        // POST: Riesgos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Riesgo riesgo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(riesgo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivoId"] = new SelectList(_context.Activos, "Id", "Nombre", riesgo.ActivoId);
            return View(riesgo);
        }
    }
}
