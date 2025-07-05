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

        // LISTAR TODOS LOS ACTIVOS
        public async Task<IActionResult> Index()
        {
            var activos = await _context.Activos.ToListAsync();
            return View(activos);
        }

        // AGRUPAR ACTIVOS POR CATEGORÍA
        public async Task<IActionResult> AgrupadosPorCategoria()
        {
            var agrupados = await _context.Activos
                .GroupBy(a => a.Categoria)
                .Select(g => new
                {
                    Categoria = g.Key,
                    Activos = g.ToList()
                })
                .ToListAsync();

            return View(agrupados);
        }

        // DETALLES DE UN ACTIVO
        public async Task<IActionResult> Details(int id)
        {
            var activo = await _context.Activos.FindAsync(id);
            if (activo == null) return NotFound();
            return View(activo);
        }

        // FORMULARIO DE CREACIÓN
        public IActionResult Create() => View();

        // GUARDAR NUEVO ACTIVO
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // FORMULARIO DE EDICIÓN
        public async Task<IActionResult> Edit(int id)
        {
            var activo = await _context.Activos.FindAsync(id);
            if (activo == null) return NotFound();
            return View(activo);
        }

        // GUARDAR CAMBIOS DE EDICIÓN
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Activo activo)
        {
            if (id != activo.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(activo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activo);
        }

        // CONFIRMACIÓN PARA ELIMINAR
        public async Task<IActionResult> Delete(int id)
        {
            var activo = await _context.Activos.FindAsync(id);
            if (activo == null) return NotFound();
            return View(activo);
        }

        // ELIMINAR ACTIVO
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
