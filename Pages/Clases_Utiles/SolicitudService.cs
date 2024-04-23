using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Proyect_two.Pages.Clases_Utiles
{
    public class SolicitudService
    {
        private readonly string connectionString;

        public SolicitudService(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Método para agregar una nueva solicitud a la base de datos
        public async Task AgregarSolicitud(Solicitud nuevaSolicitud)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Solicitudes (IdCliente, IdOpcion, DescripcionProblema, Estado, IdTecnico, Calificacion)
                                 VALUES (@IdCliente, @IdOpcion, @DescripcionProblema, @Estado, @IdTecnico, @Calificacion)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCliente", nuevaSolicitud.IdCliente);
                    command.Parameters.AddWithValue("@IdOpcion", nuevaSolicitud.IdOpcion);
                    command.Parameters.AddWithValue("@DescripcionProblema", nuevaSolicitud.DescripcionProblema);
                    command.Parameters.AddWithValue("@Estado", nuevaSolicitud.Estado);
                    command.Parameters.AddWithValue("@IdTecnico", nuevaSolicitud.IdTecnico);
                    command.Parameters.AddWithValue("@Calificacion", nuevaSolicitud.Calificacion);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para actualizar el estado de una solicitud en la base de datos
        public async Task ActualizarEstadoSolicitud(int id, string nuevoEstado)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Solicitudes SET Estado = @NuevoEstado WHERE IdSolicitud = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NuevoEstado", nuevoEstado);
                    command.Parameters.AddWithValue("@Id", id);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para eliminar una solicitud de la base de datos
        public async Task EliminarSolicitud(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Solicitudes WHERE IdSolicitud = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para buscar una solicitud por su ID
        public async Task<Solicitud> BuscarSolicitudPorId(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Solicitudes WHERE IdSolicitud = @Id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (reader.Read())
                    {
                        return new Solicitud
                        {
                            IdSolicitud = reader.GetInt32(reader.GetOrdinal("IdSolicitud")),
                            IdCliente = reader.GetInt32(reader.GetOrdinal("IdCliente")),
                            IdOpcion = reader.GetInt32(reader.GetOrdinal("IdOpcion")),
                            DescripcionProblema = reader.GetString(reader.GetOrdinal("DescripcionProblema")),
                            Estado = reader.GetString(reader.GetOrdinal("Estado")),
                            IdTecnico = reader.IsDBNull(reader.GetOrdinal("IdTecnico")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("IdTecnico")),
                            Calificacion = reader.GetString(reader.GetOrdinal("Calificacion"))
                        };
                    }

                    return null;
                }
            }
        }

        // Método para obtener todas las solicitudes de un cliente
        public async Task<ListaEnlazadaSimple> ObtenerSolicitudesPorCliente(int idCliente)
        {
            ListaEnlazadaSimple solicitudesCliente = new ListaEnlazadaSimple();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Solicitudes WHERE IdCliente = @IdCliente";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCliente", idCliente);

                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        Solicitud solicitud = new Solicitud
                        {
                            IdSolicitud = reader.GetInt32(reader.GetOrdinal("IdSolicitud")),
                            IdCliente = reader.GetInt32(reader.GetOrdinal("IdCliente")),
                            IdOpcion = reader.GetInt32(reader.GetOrdinal("IdOpcion")),
                            DescripcionProblema = reader.GetString(reader.GetOrdinal("DescripcionProblema")),
                            Estado = reader.GetString(reader.GetOrdinal("Estado")),
                            IdTecnico = reader.IsDBNull(reader.GetOrdinal("IdTecnico")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("IdTecnico")),
                            Calificacion = reader.GetString(reader.GetOrdinal("Calificacion"))
                        };

                        solicitudesCliente.AgregarAlFinal(solicitud);
                    }
                }
            }

            return solicitudesCliente;
        }
        public async Task AgregarNuevaOpcion(string descripcion)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Opciones (Descripcion) VALUES (@Descripcion)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Descripcion", descripcion);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // Método para obtener todas las opciones disponibles
        public async Task<ListaEnlazadaSimple> ObtenerOpciones()
        {
            ListaEnlazadaSimple opciones = new ListaEnlazadaSimple();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Opciones";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (reader.Read())
                    {
                        Opcion opcion = new Opcion
                        {
                            IdOpcion = reader.GetInt32(reader.GetOrdinal("IdOpcion")),
                            Descripcion = reader.GetString(reader.GetOrdinal("Descripcion"))
                        };

                        opciones.AgregarAlFinal(opcion);
                    }
                }
            }

            return opciones;
        }
    }
}

