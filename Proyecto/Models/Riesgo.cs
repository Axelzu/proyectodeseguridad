using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Models
{
    public class Riesgo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un activo")]
        public int ActivoId { get; set; }

        [ForeignKey("ActivoId")]
        public Activo Activo { get; set; }

        [Required(ErrorMessage = "La amenaza es obligatoria")]
        [StringLength(100)]
        public string Amenaza { get; set; }

        [Required(ErrorMessage = "La vulnerabilidad es obligatoria")]
        [StringLength(100)]
        public string Vulnerabilidad { get; set; }

        [StringLength(200)]
        public string ControlesExistentes { get; set; }

        [Required(ErrorMessage = "La probabilidad es obligatoria")]
        [Range(0, 1, ErrorMessage = "La probabilidad debe ser un valor entre 0 y 1")]
        public decimal Probabilidad { get; set; }  // Valor entre 0 y 1

        [Required(ErrorMessage = "El impacto es obligatorio")]
        [Range(0, 100, ErrorMessage = "El impacto debe ser un valor entre 0 y 100")]
        public decimal Impacto { get; set; } // Valor numérico del impacto

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

        [StringLength(300)]
        public string Observaciones { get; set; }
    }
}
