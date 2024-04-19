using System.Data.SqlClient;

using  Proyect_two.Pages.Menus_clientes;

namespace Proyect_two
{
    public class UsuarioService
    {
        
        private readonly string _connectionString;

        public UsuarioService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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
        public async Task<Usuario> ObtenerUsuarioPorId(int id)
        {
            Usuario usuario = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Usuarios WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.Read())
                {
                    usuario = new Usuario
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

            return usuario; // Añade esta línea para corregir el error
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

