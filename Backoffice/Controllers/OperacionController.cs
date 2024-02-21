using Backoffice.Models;
using Backoffice.Tools.DAL.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RabbitMqService.Abstractions;
using RabbitMqServiceMCGA.Queues;

namespace Backoffice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OperacionController : ControllerBase
    {
        private readonly IMessageSender _messageSender;
        public OperacionController(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        [HttpPost("pago")]
        public async Task<IActionResult> RegistrarPago([FromBody] Operacion operacionRequest)
        {

            DateTime fecha = DateTime.Now;

            try
            {
                await _messageSender.PublishAsync<Pagos, Operacion>(operacionRequest);
                SqlParameter[] parameters = new SqlParameter[]
                {

                    new SqlParameter("@IdTerminal",operacionRequest.idterminal),
                    new SqlParameter("@IdCasino",operacionRequest.idcasino),
                    new SqlParameter("@Monto",operacionRequest.monto),
                    new SqlParameter("@Fecha",fecha)

                };

                int rowsAffected = SqlHelper.ExecuteNonQuery("INSERT INTO [dbo].[Pagos] (id_terminal, id_casino, monto, fecha) VALUES (@IdTerminal, @IdCasino, @Monto, @Fecha)", System.Data.CommandType.Text, parameters);

                if (rowsAffected > 0)
                {
                    return Ok("Registro insertado correctamente");
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

        [HttpPost("deposito")]
        public async Task<IActionResult> RegistrarDeposito([FromBody] Operacion operacionRequest)
        {

            DateTime fecha = DateTime.Now;

            try
            {
                await _messageSender.PublishAsync<Depositos, Operacion>(operacionRequest);
                SqlParameter[] parameters = new SqlParameter[]
                {

                    new SqlParameter("@IdTerminal",operacionRequest.idterminal),
                    new SqlParameter("@IdCasino",operacionRequest.idcasino),
                    new SqlParameter("@Monto",operacionRequest.monto),
                    new SqlParameter("@Fecha",fecha)

                };

                int rowsAffected = SqlHelper.ExecuteNonQuery("INSERT INTO [dbo].[Depositos] (id_terminal, id_casino, monto, fecha) VALUES (@IdTerminal, @IdCasino, @Monto, @Fecha)", System.Data.CommandType.Text, parameters);

                if (rowsAffected > 0)
                {
                    return Ok("Registro insertado correctamente");
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
    }
}
