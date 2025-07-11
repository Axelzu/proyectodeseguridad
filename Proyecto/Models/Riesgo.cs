﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Models
{
    public enum NivelVulnerabilidad
    {
        [Display(Name = "Muy baja")]
        Muy_Baja,

        [Display(Name = "Baja")]
        Baja,

        [Display(Name = "Media")]
        Media,

        [Display(Name = "Alta")]
        Alta,

        [Display(Name = "Muy alta")]
        Muy_Alta
    }

    public class Riesgo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un activo")]
        public int? ActivoId { get; set; }

        [ForeignKey(nameof(ActivoId))]
        public Activo Activo { get; set; }

        [Required(ErrorMessage = "La amenaza es obligatoria")]
        [StringLength(100)]
        public string Amenaza { get; set; }

        [Required(ErrorMessage = "La vulnerabilidad es obligatoria")]
        public NivelVulnerabilidad? Vulnerabilidad { get; set; }

        [StringLength(200)]
        public string ControlesExistentes { get; set; }

        [Required(ErrorMessage = "La probabilidad es obligatoria")]
        [Range(0.0, 1.0, ErrorMessage = "La probabilidad debe estar entre 0 y 1")]
        [Column(TypeName = "decimal(3,2)")]
        public decimal Probabilidad { get; set; }

        [Required(ErrorMessage = "El impacto es obligatorio")]
        [Range(0.0, 100.0, ErrorMessage = "El impacto debe estar entre 0 y 100")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal Impacto { get; set; }

        [NotMapped]
        public decimal NivelRiesgo => Probabilidad * Impacto;

        [NotMapped]
        public string ClasificacionRiesgo
        {
            get
            {
                if (NivelRiesgo >= 70) return "Alto";
                if (NivelRiesgo >= 30) return "Medio";
                return "Bajo";
            }
        }

        public ICollection<Control> Controles { get; set; } = new List<Control>();
        public ICollection<Observacion> Observaciones { get; set; } = new List<Observacion>();
    }
}
