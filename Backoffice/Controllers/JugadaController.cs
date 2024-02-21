using Microsoft.AspNetCore.Mvc;
using Backoffice.Models;
using System.Net;
using Backoffice.Tools.DAL.Tools;
using Microsoft.Data.SqlClient;

namespace Backoffice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JugadaController : Controller
    {
        [HttpGet("resultado")]
        public IActionResult GetResultado()
        {
            
            bool resultado = ObtenerResultado();
            return Ok(resultado);
        }

        [HttpPost("apuesta")]
        public IActionResult RealizarApuesta([FromBody] Apuesta apuestaRequest)
        {
            DateTime fecha = DateTime.Now;

            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
            new SqlParameter("@id_terminal", apuestaRequest.idterminal),
            new SqlParameter("@id_casino", apuestaRequest.idcasino),
            new SqlParameter("@monto", apuestaRequest.monto),
            new SqlParameter("@fecha", fecha)
                };

                int rowsAffected = SqlHelper.ExecuteNonQuery("INSERT INTO [dbo].[Apuestas] (id_terminal, id_casino, monto, fecha) VALUES (@id_terminal, @id_casino, @monto, @fecha)", System.Data.CommandType.Text, parameters);

                if (rowsAffected > 0)
                {
                    double factor = ProcesarApuesta(); 

                    return Ok(new { Factor = factor, Mensaje = "Registro insertado correctamente" });
                }
                else
                {
                    return BadRequest("No se pudo insertar el registro");
                }

            }
            catch (Exception ex)
            {
                
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        private double ProcesarApuesta()
        {
            Random random = new Random();
            double factorAleatorio = random.NextDouble() * (3.0 - 1.5) + 1.5;

            
            return factorAleatorio;
        }

        private bool ObtenerResultado()
        {
            Random random = new Random();

            int numero = random.Next(3, 11);
            bool jugada = false;

            //true=ganador - false=perdedor


            if (numero == 9)
            {
                jugada = true;
            }
            else
            {
                jugada = false;
            }


            return jugada;
        }

        
    }
      
}
