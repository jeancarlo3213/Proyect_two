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
        public async Task<bool> ValidarDPI(string dpi)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT COUNT(*) FROM Usuarios WHERE DPI = @DPI";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DPI", dpi);

                await connection.OpenAsync();
                int count = (int)await command.ExecuteScalarAsync();
                return count > 0;
            }
        }
        public async Task<ListaEnlazadaSimple> ObtenerSolicitudesPorTecnico(int idTecnico)
        {
            ListaEnlazadaSimple solicitudesAsignadas = new ListaEnlazadaSimple();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                SELECT * FROM Solicitudes 
                WHERE IdTecnico = @IdTecnico AND Estado = 'Asignado'";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IdTecnico", idTecnico);

                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    solicitudesAsignadas.AgregarAlFinal(new Solicitud
                    {
                        IdSolicitud = reader.GetInt32(reader.GetOrdinal("IdSolicitud")),
                        IdCliente = reader.GetInt32(reader.GetOrdinal("IdCliente")),
                        IdOpcion = reader.GetInt32(reader.GetOrdinal("IdOpcion")),
                        DescripcionProblema = reader.GetString(reader.GetOrdinal("DescripcionProblema")),
                        Estado = reader.GetString(reader.GetOrdinal("Estado")),
                        IdTecnico = idTecnico,
                        Calificacion = reader.GetString(reader.GetOrdinal("Calificacion"))
                    });
                }
                reader.Close();
            }
            return solicitudesAsignadas;
        }
        public async Task AgregarUsuario(Usuario_classee user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    INSERT INTO Usuarios (Nombre, Apellido, Usuario, Contraseña, Email, Puesto, DPI, NumeroOficina)
                    VALUES (@Nombre, @Apellido, @Usuario, @Contraseña, @Email, @Puesto, @DPI, @NumeroOficina)";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Nombre", user.Nombre);
                command.Parameters.AddWithValue("@Apellido", user.Apellido);
                command.Parameters.AddWithValue("@Usuario", user.Usuario);
                command.Parameters.AddWithValue("@Contraseña", user.Contraseña);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Puesto", user.Puesto);
                command.Parameters.AddWithValue("@DPI", user.DPI);
                command.Parameters.AddWithValue("@NumeroOficina", user.NumeroOficina);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
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

        public async Task<ListaEnlazadaSimple> SearchUsersById(string id)
        {
            ListaEnlazadaSimple results = new ListaEnlazadaSimple();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Usuarios WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    results.AgregarAlFinal(new Usuario_classee
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                        Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Puesto = reader.GetString(reader.GetOrdinal("Puesto")),
                        DPI = reader.GetString(reader.GetOrdinal("DPI")),
                        NumeroOficina = reader.GetInt32(reader.GetOrdinal("NumeroOficina"))
                    });
                }
                reader.Close();
            }
            return results;
        }

        // Método para buscar usuarios por DPI
        public async Task<ListaEnlazadaSimple> SearchUsersByDPI(string dpi)
        {
            ListaEnlazadaSimple results = new ListaEnlazadaSimple();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Usuarios WHERE DPI = @DPI";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DPI", dpi);

                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    results.AgregarAlFinal(new Usuario_classee
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                        Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Puesto = reader.GetString(reader.GetOrdinal("Puesto")),
                        DPI = reader.GetString(reader.GetOrdinal("DPI")),
                        NumeroOficina = reader.GetInt32(reader.GetOrdinal("NumeroOficina"))
                    });
                }
                reader.Close();
            }
            return results;
        }

        // Método para buscar usuarios por puesto
        public async Task<ListaEnlazadaSimple> SearchUsersByPuesto(string puesto)
        {
            ListaEnlazadaSimple results = new ListaEnlazadaSimple();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Usuarios WHERE Puesto = @Puesto";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Puesto", puesto);

                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    results.AgregarAlFinal(new Usuario_classee
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                        Usuario = reader.GetString(reader.GetOrdinal("Usuario")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Puesto = reader.GetString(reader.GetOrdinal("Puesto")),
                        DPI = reader.GetString(reader.GetOrdinal("DPI")),
                        NumeroOficina = reader.GetInt32(reader.GetOrdinal("NumeroOficina"))
                    });
                }
                reader.Close();
            }
            return results;
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

