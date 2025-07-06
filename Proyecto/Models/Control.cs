using System.ComponentModel.DataAnnotations;

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
    }
}
 