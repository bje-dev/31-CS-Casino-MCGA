using Api.Receiver;
using BLL.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Domain.RabbitMessages;
using Microsoft.AspNetCore.Mvc;
using RabbitMqService.RabbitMq;
using System.Linq;
using System.Threading;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PagosController : ControllerBase
    {
        private readonly IPagosService _pagosService;
        private readonly MessageManager _messageManager;
        private readonly PagosReceiver<PagosMessage> _pagosReceiver;
        public PagosController(IPagosService pagosService, MessageManager messageManager, PagosReceiver<PagosMessage> pagosReceiver)
        {
            _pagosService = pagosService;
            _messageManager = messageManager;
            _pagosReceiver = pagosReceiver;
        }

        [HttpPost]
        [Route("GenerarPago")]
        public async Task<IActionResult> CrearPago(PagosModel pagosModel)
        {
            try
            {
                await _pagosService.CrearPago(pagosModel);
                return Ok();
            }
            catch (PagosException ex)
            {
                return BadRequest(new { Mensaje = ex.Message});
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("ObtenerPagos")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _pagosService.ObtenerPagos();            
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }
    }
}
