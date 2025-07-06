using System;
using System.Collections.Generic;

namespace Proyecto.Models
{
    public class RiesgosIndexViewModel
    {
        public IEnumerable<Riesgo> Riesgos { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public string Search { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
