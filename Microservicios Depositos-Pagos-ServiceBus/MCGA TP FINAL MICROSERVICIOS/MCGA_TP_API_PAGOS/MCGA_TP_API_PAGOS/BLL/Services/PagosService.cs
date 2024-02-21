using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Repositories;
using Domain.Models;
using Domain.RabbitMessages;
using RabbitMqService.Abstractions;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class PagosService : IMessageReceiver<PagosMessage>, IPagosService
    {
        private readonly IPagosRepository _pagosRepository;
        public PagosService(IPagosRepository pagosRepository)
        {
            _pagosRepository = pagosRepository;
        }

        public async Task CrearPago(PagosModel pagosModel) => await _pagosRepository.Insert(pagosModel);

        public async Task<IEnumerable<PagosModel>> ObtenerPagos() => await _pagosRepository.GetAll();

        public async Task ReceiveAsync(PagosMessage message, CancellationToken cancellationToken)
        {
            //_logger.LogInformation("Mensaje recibido de respuesta");
            try
            {
                PagosModel pagosModel = new();
                pagosModel.FechaPago = message.fecha;
                pagosModel.Monto = message.monto;
                pagosModel.IdTerminal = message.idterminal;
                pagosModel.IdCasino = message.idcasino;
                await CrearPago(pagosModel);
            }
            catch (Exception ex)
            {

            }
            //_logger.LogInformation($"Respuesta procesada");
        }
    }
}
