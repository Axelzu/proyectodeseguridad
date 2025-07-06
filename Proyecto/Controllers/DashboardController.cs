using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context) => _context = context;

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            // Traemos todos los riesgos con sus controles
            var riesgos = await _context.Riesgos
                .Include(r => r.Controles)
                .ToListAsync();

            // Calculamos totales
            var total = riesgos.Count;
            var alto = riesgos.Count(r => r.NivelRiesgo >= 9);
            var medio = riesgos.Count(r => r.NivelRiesgo >= 4 && r.NivelRiesgo < 9);
            var bajo = total - alto - medio;

            // Preparamos las filas de la tabla (solo top 10 por riesgo original)
            var items = riesgos
                .OrderByDescending(r => r.NivelRiesgo)
                .Take(10)
                .Select(r => new RiesgoDashboardItem
                {
                    Id = r.Id,
                    ActivoNombre = r.Activo.Nombre,
                    Amenaza = r.Amenaza,
                    NivelRiesgo = r.NivelRiesgo,
                    ControlesCount = r.Controles.Count,
                    NivelResidual = r.Controles.Any()
                        ? r.Controles.Min(c => c.NivelRiesgoResidual)
                        : r.NivelRiesgo
                });

            var vm = new DashboardViewModel
            {
                TotalRiesgos = total,
                RiesgosAlto = alto,
                RiesgosMedio = medio,
                RiesgosBajo = bajo,
                Items = items
            };

            return View(vm);
        }
    }
}
