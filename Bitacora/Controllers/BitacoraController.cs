using Bitacora.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;


namespace Bitacora.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class BitacoraController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public BitacoraController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
              
        [HttpPost]
        public IActionResult PostBitacora([FromBody] Registro bitacora)
        {
            // Traigo cadena de conexión desde appsettings.json
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Crear la consulta SQL de inserción
                    string insertQuery = "INSERT INTO Bitacora (datetime, operacion, componente, casino, descripcion, resultado) " +
                                         "VALUES (@DateTime, @Operacion, @Componente, @Casino, @Descripcion, @Resultado)";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        // Obtener la fecha y hora actual en C#
                        DateTime fechaHoraActual = DateTime.Now;

                        // Establecer los parámetros de la consulta
                        cmd.Parameters.AddWithValue("@DateTime", fechaHoraActual);
                        cmd.Parameters.AddWithValue("@Operacion", bitacora.Operacion);
                        cmd.Parameters.AddWithValue("@Componente", bitacora.Componente);
                        cmd.Parameters.AddWithValue("@Casino", bitacora.Casino);
                        cmd.Parameters.AddWithValue("@Descripcion", bitacora.Descripcion);
                        cmd.Parameters.AddWithValue("@Resultado", bitacora.Resultado);

                        // Ejecutar la consulta de inserción
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok("Registro insertado correctamente");
                        }
                        else
                        {
                            return BadRequest("No se pudo insertar el registro");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
