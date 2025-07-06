// Models/Observacion.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Models
{
    public class Observacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RiesgoId { get; set; }
        public Riesgo Riesgo { get; set; }

        [Required, StringLength(300)]
        public string Texto { get; set; }

        [Required, StringLength(100)]
        public string Autor { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
