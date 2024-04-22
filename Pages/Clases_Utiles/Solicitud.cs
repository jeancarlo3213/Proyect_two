namespace Proyect_two.Pages.Clases_Utiles
{
    public class Solicitud
    {
        public int Id { get; set; }
        public int IdOpcion { get; set; }
        public int IdCliente { get; set; }
        public string Estado { get; set; }
        public int? IdTecnico { get; set; }
        public string Calificacion { get; set; }
    }
}
