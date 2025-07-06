using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class ObservacionesController : Controller
    {
        private readonly AppDbContext _context;
        public ObservacionesController(AppDbContext context) => _context = context;

        // GET: Observaciones/Create?r=5
        public IActionResult Create(int r)
        {
            ViewBag.RiesgoId = r;
            return View(new Observacion { RiesgoId = r });
        }

        // POST: Observaciones/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Observacion obs)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.RiesgoId = obs.RiesgoId;
                return View(obs);
            }

            _context.Observaciones.Add(obs);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Riesgos", new { id = obs.RiesgoId });
        }

        // GET: Observaciones/Index?r=5
        public async Task<IActionResult> Index(int r)
        {
            ViewBag.RiesgoId = r;
            var list = await _context.Observaciones
                             .Where(o => o.RiesgoId == r)
                             .OrderByDescending(o => o.Fecha)
                             .ToListAsync();
            return View(list);
        }
    }
}
