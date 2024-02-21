using BLL.Interfaces;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using RabbitMqService.RabbitMq;
using System.Linq;
using System.Threading;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DepositosController : ControllerBase
    {
        private readonly IDepositosService _depositosService;
        private readonly MessageManager _messageManager;

        public DepositosController(IDepositosService DepositosService, MessageManager messageManager)
        {
            _depositosService = DepositosService;
            _messageManager = messageManager;
        }

        [HttpPost]
        [Route("GenerarDeposito")]
        public async Task<IActionResult> CrearPago(DepositosModel DepositosModel)
        {
            try
            {
                await _depositosService.CrearDeposito(DepositosModel);
                return Ok();
            }
            catch (DepositosException ex)
            {
                return BadRequest(new { Mensaje = ex.Message});
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("ObtenerDepositos")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _depositosService.ObtenerDepositos();            
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mensaje = ex.Message });
            }
        }
    }
}
