using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class ControlesController : Controller
    {
        private readonly AppDbContext _context;
        public ControlesController(AppDbContext context) => _context = context;

        // GET: Tratamientos/Create
        public IActionResult Create(int? riesgoId)
        {
            ViewBag.Riesgos = new SelectList(
                _context.Riesgos.Include(r => r.Activo)
                                .Select(r => new { r.Id, Desc = $"{r.Activo.Nombre} – {r.Amenaza}" }),
                "Id", "Desc", riesgoId);

            ViewBag.Estrategias = new SelectList(
                Enum.GetValues(typeof(EstrategiaRespuesta))
                    .Cast<EstrategiaRespuesta>()
                    .Select(e => new { Value = e, Text = e.ToString() }),
                "Value", "Text");

            return View();
        }

        // POST: Tratamientos/Create

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Control model)
        {
            if (!ModelState.IsValid)
            {
                // volver a cargar dropdowns
                return Create(model.RiesgoId);
            }

            _context.Controles.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Riesgos", new { id = model.RiesgoId });
        }

        // GET: Tratamientos/Index/{riesgoId}
        public async Task<IActionResult> Index(int riesgoId)
        {
            ViewBag.RiesgoId = riesgoId;
            var lista = await _context.Controles
                .Where(t => t.RiesgoId == riesgoId)
                .ToListAsync();
            return View(lista);
        }
    }
}
