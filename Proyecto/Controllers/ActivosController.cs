using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class ActivosController : Controller
    {
        private readonly AppDbContext _context;

        public ActivosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _context.Activos.ToListAsync());

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Activo activo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activo);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var activo = await _context.Activos.FindAsync(id);
            if (activo != null)
            {
                _context.Activos.Remove(activo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
