using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto.Models
{
    public class Control
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RiesgoId { get; set; }
        public Riesgo Riesgo { get; set; }

        [Required]
        public EstrategiaRespuesta Estrategia { get; set; }

        [Required, StringLength(300)]
        public string ControlesPropuestos { get; set; }

        [Required, StringLength(100)]
        public string Responsable { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaPropuesta { get; set; } = DateTime.Now;

        // NUEVOS CAMPOS: porcentaje de reducción (0.0–1.0)
        [Required, Range(0.0, 1.0)]
        [Column(TypeName = "decimal(3,2)")]
        public decimal EfectividadProbabilidad { get; set; }

        [Required, Range(0.0, 1.0)]
        [Column(TypeName = "decimal(3,2)")]
        public decimal EfectividadImpacto { get; set; }

        // CÁLCULOS NO MAPEADOS
        [NotMapped]
        public decimal ProbabilidadResidual
            => Math.Round(Riesgo.Probabilidad * (1 - EfectividadProbabilidad), 2);

        [NotMapped]
        public decimal ImpactoResidual
            => Math.Round(Riesgo.Impacto * (1 - EfectividadImpacto), 2);

        [NotMapped]
        public decimal NivelRiesgoResidual
            => Math.Round(ProbabilidadResidual * ImpactoResidual, 2);

        [NotMapped]
        public string ClasificacionResidual
        {
            get
            {
                var r = NivelRiesgoResidual;
                if (r >= 70) return "Alto";
                if (r >= 30) return "Medio";
                return "Bajo";
            }
        }
    }
}
 