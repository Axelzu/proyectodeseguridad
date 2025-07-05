using System.ComponentModel.DataAnnotations;

namespace Proyecto.Models
{
    public class Activo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del activo es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El valor debe ser mayor a 0")]
        public decimal Valor { get; set; }

        [Required]
        public string Categoria { get; set; }

        public bool ActivoDisponible { get; set; } = true;
    }
}
