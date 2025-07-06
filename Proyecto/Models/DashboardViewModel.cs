using System.Collections.Generic;

namespace Proyecto.Models
{
    public class DashboardViewModel
    {
        // Totales por nivel
        public int TotalRiesgos { get; set; }
        public int RiesgosAlto { get; set; }
        public int RiesgosMedio { get; set; }
        public int RiesgosBajo { get; set; }

        // Lista simplificada de riesgos para la tabla
        public IEnumerable<RiesgoDashboardItem> Items { get; set; }
    }

    public class RiesgoDashboardItem
    {
        public int Id { get; set; }
        public string ActivoNombre { get; set; }
        public string Amenaza { get; set; }
        public decimal NivelRiesgo { get; set; }
        public int ControlesCount { get; set; }
        public decimal NivelResidual { get; set; }
    }
}
