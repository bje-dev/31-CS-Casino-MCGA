using Api.Receiver;
using BLL.Interfaces;
using Domain.RabbitMessages;
using Microsoft.AspNetCore.Mvc;
using RabbitMqService.RabbitMq;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly MessageManager _messageManager;
        private readonly DepositosReceiver<DepositosMessage> _depositosReceiver;
        public StatusController(MessageManager messageManager, DepositosReceiver<DepositosMessage> depositosReceiver)
        {
            _messageManager = messageManager;
            _depositosReceiver = depositosReceiver;
        }

        [HttpGet]
        [Route("VerEstado")]
        public async Task<IActionResult> VerEstado()
        {
            try
            {
                return Ok("La api esta habilitada para su uso.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }


        [HttpPost]
        [Route("FrenarOperaciones")]
        public async Task<IActionResult> FrenarOperaciones()
        {
            try
            {
                _messageManager.Channel.Close();
                return Ok("Servicio de RabbitMQ frenado");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }

        [HttpPost]
        [Route("ReanudarOperaciones")]
        public async Task<IActionResult> ReanudarOperaciones()
        {
            try
            {
                await _depositosReceiver.StartAsync(CancellationToken.None);
                return Ok("Servicio de RabbitMQ restablecido");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }
    }
}
