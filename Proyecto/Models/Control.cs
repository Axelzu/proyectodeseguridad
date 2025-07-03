namespace Proyecto.Models
{
    public class Control
    {
        public int Id { get; set; }
        public int RiesgoId { get; set; }
        public string Estrategia { get; set; } = ""; // Mitigar, Aceptar, Transferir, Evitar
        public string Descripcion { get; set; } = "";
        public string Responsable { get; set; } = "";
        public Riesgo? Riesgo { get; set; }
    }
}
