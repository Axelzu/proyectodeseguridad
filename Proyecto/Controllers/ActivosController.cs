using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto.Controllers
{
    public class ActivosController : Controller
    {
        private readonly AppDbContext _context;

        public ActivosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Activos
        public async Task<IActionResult> Index()
        {
            var activos = await _context.Activos.ToListAsync();
            return View(activos);
        }

        // GET: Activos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var activo = await _context.Activos.FindAsync(id);
            if (activo == null)
                return NotFound();

            return View(activo);
        }

        // GET: Activos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Activo activo)
        {
            if (!ModelState.IsValid)
                return View(activo);

            _context.Add(activo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Activos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var activo = await _context.Activos.FindAsync(id);
            if (activo == null)
                return NotFound();

            return View(activo);
        }

        // POST: Activos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,Categoria,Valor,ActivoDisponible")] Activo activo)
        {
            if (id != activo.Id)
                return NotFound();

            var activoDb = await _context.Activos.FindAsync(id);
            if (activoDb == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(activo);

            // Actualizamos solo los campos editables
            activoDb.Nombre = activo.Nombre;
            activoDb.Descripcion = activo.Descripcion;
            activoDb.Categoria = activo.Categoria;
            activoDb.Valor = activo.Valor;
            activoDb.ActivoDisponible = activo.ActivoDisponible;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActivoExists(activo.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Activos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var activo = await _context.Activos.FindAsync(id);
            if (activo == null)
                return NotFound();

            return View(activo);
        }

        // POST: Activos/Delete/5
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

        private bool ActivoExists(int id)
        {
            return _context.Activos.Any(e => e.Id == id);
        }
    }
}
