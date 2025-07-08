using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class RiesgosController : Controller
    {
        private readonly AppDbContext _context;
        public RiesgosController(AppDbContext context) => _context = context;

        // Helper: carga ViewBag.ActivosList con items "Id – Nombre"
        private void PopulateActivosDropDown(int? selectedId = null)
        {
            var items = _context.Activos
                                .OrderBy(a => a.Id)
                                .Select(a => new SelectListItem
                                {
                                    Value = a.Id.ToString(),
                                    Text = $"{a.Id} – {a.Nombre}",
                                    Selected = (a.Id == selectedId)
                                })
                                .ToList();

            ViewBag.ActivosList = items;
        }

        // GET: Riesgos
        public async Task<IActionResult> Index(string search, int page = 1, int pageSize = 10)
        {
            var query = _context.Riesgos.Include(r => r.Activo).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(r =>
                    r.Activo.Nombre.Contains(search) ||
                    r.Amenaza.Contains(search));

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

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
            PopulateActivosDropDown();
            return View();
        }

        // POST: Riesgos/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Riesgo riesgo)
        {
            if (!ModelState.IsValid)
            {
                PopulateActivosDropDown(riesgo.ActivoId);
                return View(riesgo);
            }

            _context.Riesgos.Add(riesgo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Riesgos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var riesgo = await _context.Riesgos.FindAsync(id.Value);
            if (riesgo == null) return NotFound();

            PopulateActivosDropDown(riesgo.ActivoId);
            return View(riesgo);
        }

        // POST: Riesgos/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Riesgo riesgo)
        {
            if (id != riesgo.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                PopulateActivosDropDown(riesgo.ActivoId);
                return View(riesgo);
            }

            _context.Riesgos.Update(riesgo);
            await _context.SaveChangesAsync();
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
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
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
    }
}
