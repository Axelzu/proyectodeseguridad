namespace Proyecto.Models
{
    public class Activo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public string Tipo { get; set; } = ""; // Físico, Software, Red
        public string ClasificacionCIA { get; set; } = "";
        public decimal Valor { get; set; }
    }
}
