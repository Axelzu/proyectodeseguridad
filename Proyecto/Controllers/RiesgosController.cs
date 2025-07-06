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
        public async Task<IActionResult> Index(string search, int page = 1, int pageSize = 10)
        {
            // 1) Base query con Include
            var query = _context.Riesgos
                                .Include(r => r.Activo)
                                .AsQueryable();

            // 2) Filtro
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r =>
                    r.Activo.Nombre.Contains(search) ||
                    r.Amenaza.Contains(search));
            }

            // 3) Conteo total
            var total = await query.CountAsync();

            // 4) Paginación
            var items = await query
                .OrderBy(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 5) Construir ViewModel
            var vm = new RiesgosIndexViewModel
            {
                Riesgos = items,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = total,
                Search = search
            };

            return View(vm);
        }


        // GET: Riesgos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var riesgo = await _context.Riesgos
                                       .Include(r => r.Activo)
                                       .FirstOrDefaultAsync(r => r.Id == id.Value);
            if (riesgo == null) return NotFound();

            return View(riesgo);
        }

        // GET: Riesgos/Create
        public IActionResult Create()
        {
            ViewData["ActivoId"] =
                new SelectList(_context.Activos, "Id", "Nombre");
            return View();
        }

        // POST: Riesgos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Riesgo riesgo)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ActivoId"] =
                    new SelectList(_context.Activos, "Id", "Nombre", riesgo.ActivoId);
                return View(riesgo);
            }

            _context.Add(riesgo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Riesgos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var riesgo = await _context.Riesgos.FindAsync(id.Value);
            if (riesgo == null) return NotFound();

            ViewBag.Activos = new SelectList(
                _context.Activos,
                "Id",
                "Nombre",
                riesgo.ActivoId);

            return View(riesgo);
        }


        // POST: Riesgos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Riesgo riesgo)
        {
            if (id != riesgo.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["ActivoId"] =
                    new SelectList(_context.Activos, "Id", "Nombre", riesgo.ActivoId);
                return View(riesgo);
            }

            try
            {
                _context.Update(riesgo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RiesgoExists(riesgo.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Riesgos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var riesgo = await _context.Riesgos
                                       .Include(r => r.Activo)
                                       .FirstOrDefaultAsync(r => r.Id == id.Value);
            if (riesgo == null) return NotFound();

            return View(riesgo);
        }

        // POST: Riesgos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var riesgo = await _context.Riesgos.FindAsync(id);
            if (riesgo != null)
            {
                _context.Riesgos.Remove(riesgo);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RiesgoExists(int id)
        {
            return _context.Riesgos.Any(e => e.Id == id);
        }
    }
}
