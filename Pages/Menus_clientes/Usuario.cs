namespace Proyect_two.Pages.Menus_clientes
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string UsuarioNombre { get; set; } // Cambiado a UsuarioNombre para evitar conflicto con el nombre de la clase
        public string Contraseña { get; set; }
        public string Email { get; set; }
        public string Puesto { get; set; }
        public string DPI { get; set; }
    }
}
