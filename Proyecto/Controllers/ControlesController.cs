using System;
using System.Linq;
using System.Threading.Tasks;
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

        // Helpers para poblar los dropdowns de Riesgos y Estrategias
        private void PopulateDropdowns(int? selectedRiesgo = null)
        {
            ViewBag.Riesgos = new SelectList(
                _context.Riesgos.Include(r => r.Activo)
                                .Select(r => new { r.Id, Desc = $"{r.Activo.Nombre} – {r.Amenaza}" }),
                "Id", "Desc", selectedRiesgo);

            ViewBag.Estrategias = new SelectList(
                Enum.GetValues(typeof(EstrategiaRespuesta))
                    .Cast<EstrategiaRespuesta>()
                    .Select(e => new { Value = e, Text = e.ToString() }),
                "Value", "Text");
        }

        // GET: Controles/Create
        public IActionResult Create(int? riesgoId)
        {
            PopulateDropdowns(riesgoId);
            return View();
        }

        // POST: Controles/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Control model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns(model.RiesgoId);
                return View(model);
            }

            _context.Controles.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Riesgos", new { id = model.RiesgoId });
        }

        // GET: Controles/Index?riesgoId=5
        public async Task<IActionResult> Index(int riesgoId)
        {
            ViewBag.RiesgoId = riesgoId;
            var lista = await _context.Controles
                                     .Include(t => t.Riesgo)
                                     .ThenInclude(r => r.Activo)
                                     .Where(t => t.RiesgoId == riesgoId)
                                     .ToListAsync();
            return View(lista);
        }

        // (Opcional) GET: Controles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var control = await _context.Controles.FindAsync(id.Value);
            if (control == null) return NotFound();

            PopulateDropdowns(control.RiesgoId);
            return View(control);
        }

        // (Opcional) POST: Controles/Edit/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Control model)
        {
            if (id != model.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                PopulateDropdowns(model.RiesgoId);
                return View(model);
            }

            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!_context.Controles.Any(e => e.Id == model.Id))
            {
                return NotFound();
            }

            return RedirectToAction("Index", new { riesgoId = model.RiesgoId });
        }

        // (Opcional) GET: Controles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var control = await _context.Controles
                                        .Include(t => t.Riesgo)
                                        .ThenInclude(r => r.Activo)
                                        .FirstOrDefaultAsync(t => t.Id == id.Value);
            if (control == null) return NotFound();

            return View(control);
        }

        // (Opcional) POST: Controles/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var control = await _context.Controles.FindAsync(id);
            if (control != null)
            {
                _context.Controles.Remove(control);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", new { riesgoId = control.RiesgoId });
        }
    }
}
