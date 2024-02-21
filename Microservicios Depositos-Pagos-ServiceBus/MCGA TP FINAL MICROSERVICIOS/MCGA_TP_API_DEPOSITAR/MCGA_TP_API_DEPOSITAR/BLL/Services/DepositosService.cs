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
    public class DepositosService : IMessageReceiver<DepositosMessage>, IDepositosService
    {
        private readonly IDepositosRepository _depositosRepository;
        public DepositosService(IDepositosRepository depositosRepository)
        {
            _depositosRepository = depositosRepository;
        }

        public async Task CrearDeposito(DepositosModel depositosModel) => await _depositosRepository.Insert(depositosModel);

        public async Task<IEnumerable<DepositosModel>> ObtenerDepositos() => await _depositosRepository.GetAll();

        public async Task ReceiveAsync(DepositosMessage message, CancellationToken cancellationToken)
        {
            //_logger.LogInformation("Mensaje recibido de respuesta");
            try
            {
                DepositosModel DepositoModel = new();
                DepositoModel.FechaPago = DateTime.Now;
                DepositoModel.Monto = message.monto;
                DepositoModel.IdTerminal = message.idterminal;
                DepositoModel.IdCasino = message.idcasino;
                await CrearDeposito(DepositoModel);
            }
            catch (Exception ex)
            {

            }
            //_logger.LogInformation($"Respuesta procesada");
        }
    }
}
