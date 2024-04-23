namespace Proyect_two.Pages.Clases_Utiles
{
    public class Solicitud
    {
        public int IdSolicitud { get; set; } // Modificado para coincidir con la tabla SQL
        public int IdOpcion { get; set; }
        public int IdCliente { get; set; }
        public string DescripcionProblema { get; set; } // Modificado para coincidir con la tabla SQL
        public string Estado { get; set; }
        public int? IdTecnico { get; set; }
        public string Calificacion { get; set; }
    }
}
