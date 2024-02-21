using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using webhook;
using ConsoleApp1;
using webhook.Dominio;
using Newtonsoft.Json;
using ConsoleApp1.Dominio;

namespace APIRest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class webhookController : ControllerBase
    {
        private readonly ILogger<webhookController> _logger;
        private readonly IConfiguration _configuration;

        public webhookController(ILogger<webhookController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }



        [HttpPost("UpdateMaxMin")]
        public IActionResult Post(MaxMin data)
        {
            string toServer = "";
            try
            {

                if (data == null)
                {
                    return BadRequest("Data invalida");
                }

                Terminal rt = new Terminal();

                rt.accion = "actualizarMaxMin";

                if (data.maximo > data.minimo)
                {
                    rt.maximo = data.maximo;
                    rt.minimo = data.minimo;

                    toServer = JsonConvert.SerializeObject(rt);
                    webhook.caller.ExecuteClient(toServer);

                    return Ok();
                }
                else
                {
                    return BadRequest("El maximo debe ser mas grande que el minimo");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("BajarTerminal")]
        public IActionResult Post(BajarTerminal data)
        {
            string toServer = "";
            try
            {

                if (data == null)
                {
                    return BadRequest("Data invalida");
                }

                Terminal t = new Terminal();
                t.nroMaquina = data.idTerminal;
                t.accion = "terminalOffline";
                toServer = JsonConvert.SerializeObject(t);

                webhook.caller.ExecuteClient(toServer);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
