using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class ControlesController : Controller
    {
        private readonly AppDbContext _context;

        public ControlesController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var controles = _context.Controles.Include(c => c.Riesgo).ThenInclude(r => r.Activo);
            return View(await controles.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.Riesgos = _context.Riesgos.Include(r => r.Activo).ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Control control)
        {
            if (ModelState.IsValid)
            {
                _context.Add(control);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Riesgos = _context.Riesgos.Include(r => r.Activo).ToList();
            return View(control);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var control = await _context.Controles.FindAsync(id);
            if (control != null)
            {
                _context.Controles.Remove(control);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
