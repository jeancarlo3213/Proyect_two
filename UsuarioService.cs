using Microsoft.Extensions.Configuration;
using Proyect_two.Pages.Clases_Utiles;
using Proyect_two.Pages.Menus_clientes;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Proyect_two
{
    public class UsuarioService
    {
        private readonly string _connectionString;

        public UsuarioService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Usuario_classee> ObtenerUsuarioPorNombreUsuario(string nombreUsuario)
        {
            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                // Si el nombre de usuario es nulo o está vacío, no se puede ejecutar la consulta
                return null;
            }

            Usuario_classee usuario = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Usuarios WHERE Usuario = @Usuario";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Usuario", nombreUsuario);

                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    usuario = new Usuario_classee
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        // Agrega otros campos del usuario según sea necesario
                    };
                }

                reader.Close();
            }

            return usuario;
        }

        public async Task<ListaEnlazadaSimple> ObtenerTecnicos()
        {
            ListaEnlazadaSimple tecnicos = new ListaEnlazadaSimple();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Usuarios WHERE Puesto = 'Tecnico'";
                SqlCommand command = new SqlCommand(query, connection);

                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Usuario_classee tecnico = new Usuario_classee
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                        Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Puesto = reader.GetString(reader.GetOrdinal("Puesto")),
                        DPI = reader.GetString(reader.GetOrdinal("DPI")),
                        NumeroOficina = reader.GetInt32(reader.GetOrdinal("NumeroOficina"))
                    };
                    tecnicos.AgregarAlFinal(tecnico);
                }
                reader.Close();
            }
            return tecnicos;
        }
        public async Task<bool> ValidarCredencialesCliente(string usuario, string contraseña)
        {
            bool credencialesValidas = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Usuarios WHERE Usuario = @Usuario AND Contraseña = @Contraseña AND Puesto = 'Cliente'";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Usuario", usuario);
                command.Parameters.AddWithValue("@Contraseña", contraseña);

                await connection.OpenAsync();
                int count = (int)await command.ExecuteScalarAsync();
                credencialesValidas = count > 0;
            }

            return credencialesValidas;
        }

        public async Task<bool> ValidarCredencialesTecnico(string usuario, string contraseña)
        {
            bool credencialesValidas = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Usuarios WHERE Usuario = @Usuario AND Contraseña = @Contraseña AND Puesto = 'Tecnico'";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Usuario", usuario);
                command.Parameters.AddWithValue("@Contraseña", contraseña);

                await connection.OpenAsync();
                int count = (int)await command.ExecuteScalarAsync();
                credencialesValidas = count > 0;
            }

            return credencialesValidas;
        }

        public async Task<bool> ValidarCredencialesJefe(string usuario, string contraseña)
        {
            bool credencialesValidas = false;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM Usuarios WHERE Usuario = @Usuario AND Contraseña = @Contraseña AND Puesto = 'Jefe'";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Usuario", usuario);
                command.Parameters.AddWithValue("@Contraseña", contraseña);

                await connection.OpenAsync();
                int count = (int)await command.ExecuteScalarAsync();
                credencialesValidas = count > 0;
            }

            return credencialesValidas;
        }
    }
}

