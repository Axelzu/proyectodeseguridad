using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public ActivosController(AppDbContext context) => _context = context;

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
            if (activo == null) return NotFound();
            return View(activo);
        }

        // GET: Activos/Create
        public IActionResult Create() => View();

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

        //Post: Activos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Categoria")] Activo activo)
        {
            if (id != activo.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(activo);

            try
            {
                _context.Update(activo);
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

        private bool ActivoExists(int id)
        {
            return _context.Activos.Any(e => e.Id == id);
        }


        // GET: Activos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var activo = await _context.Activos.FindAsync(id);
            if (activo == null) return NotFound();
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

        // Helper para llenar ViewBag.ActivosList
        private async Task PopulateActivosDropDown(int? selectedId)
        {
            var lista = await _context.Activos
                                     .OrderBy(a => a.Nombre)
                                     .ToListAsync();
            ViewBag.ActivosList = new SelectList(lista, "Id", "Nombre", selectedId);
        }
    }
}
