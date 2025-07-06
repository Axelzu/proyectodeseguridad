// Controllers/ReportsController.cs
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto.Data;

namespace Proyecto.Controllers
{
    public class ReportsController : Controller
    {
        private readonly AppDbContext _context;
        public ReportsController(AppDbContext ctx) => _context = ctx;

        // GET: Reports/RiesgosCsv
        public async Task<FileResult> RiesgosCsv()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Activo;Amenaza;Vulnerability;Prob;Impact;Nivel;Clasif;Tratamiento;EfectProb;EfectImp;ResNivel;ResClasif;Observaciones");

            var lista = await _context.Riesgos
                .Include(r => r.Activo)
                .Include(r => r.Controles)
                .Include(r => r.Observaciones)
                .ToListAsync();

            foreach (var r in lista)
            {
                // Agrupamos tratamientos y observaciones en una línea
                var tratamientos = string.Join("|", r.Controles.Select(t => t.ControlesPropuestos));
                var observs = string.Join("|", r.Observaciones.Select(o => o.Texto));

                sb.AppendLine($@"""{r.Activo.Nombre}"";""{r.Amenaza}"";""{r.Vulnerabilidad}"";{r.Probabilidad};{r.Impacto};{r.NivelRiesgo};{r.ClasificacionRiesgo};""{tratamientos}"";{(r.Controles.Any() ? r.Controles.Max(t => t.EfectividadProbabilidad) : 0)};{(r.Controles.Any() ? r.Controles.Max(t => t.EfectividadImpacto) : 0)};{(r.Controles.Any() ? r.Controles.Max(t => t.NivelRiesgoResidual) : 0)};""{(r.Controles.Any() ? r.Controles.Max(t => t.ClasificacionResidual) : "")}"";""{observs}""");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", "Reporte_Riesgos.csv");
        }
    }
}
