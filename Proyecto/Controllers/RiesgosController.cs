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

        private void PopulateActivosDropDown(int? selectedId = null)
        {
            ViewBag.ActivosList = _context.Activos
                .OrderBy(a => a.Nombre)
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Nombre,
                    Selected = (a.Id == selectedId)
                })
                .ToList();
        }

        private void PopulateVulnerabilidadesDropDown(NivelVulnerabilidad? selected = null)
        {
            var list = new SelectList(
                System.Enum.GetValues(typeof(NivelVulnerabilidad))
                    .Cast<NivelVulnerabilidad>()
                    .Select(e => new { Value = e, Text = e.ToString().Replace("_", " ") }),
                "Value",
                "Text",
                selected.HasValue ? selected.Value : (NivelVulnerabilidad?)null
            );

            ViewBag.VulnerabilidadesList = list;
        }


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

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var riesgo = await _context.Riesgos
                .Include(r => r.Activo)
                .FirstOrDefaultAsync(r => r.Id == id.Value);

            if (riesgo == null) return NotFound();
            return View(riesgo);
        }

        public IActionResult Create()
        {
            PopulateActivosDropDown();
            PopulateVulnerabilidadesDropDown();
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Riesgo riesgo)
        {
            if (!ModelState.IsValid)
            {
                PopulateActivosDropDown(riesgo.ActivoId);
                PopulateVulnerabilidadesDropDown(riesgo.Vulnerabilidad);
                return View(riesgo);
            }

            _context.Riesgos.Add(riesgo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var riesgo = await _context.Riesgos.FindAsync(id.Value);
            if (riesgo == null) return NotFound();

            PopulateActivosDropDown(riesgo.ActivoId);
            PopulateVulnerabilidadesDropDown(riesgo.Vulnerabilidad);
            return View(riesgo);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Riesgo riesgo)
        {
            if (id != riesgo.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                PopulateActivosDropDown(riesgo.ActivoId);
                PopulateVulnerabilidadesDropDown(riesgo.Vulnerabilidad);
                return View(riesgo);
            }

            _context.Riesgos.Update(riesgo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var riesgo = await _context.Riesgos
                .Include(r => r.Activo)
                .FirstOrDefaultAsync(r => r.Id == id.Value);

            if (riesgo == null) return NotFound();
            return View(riesgo);
        }

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
