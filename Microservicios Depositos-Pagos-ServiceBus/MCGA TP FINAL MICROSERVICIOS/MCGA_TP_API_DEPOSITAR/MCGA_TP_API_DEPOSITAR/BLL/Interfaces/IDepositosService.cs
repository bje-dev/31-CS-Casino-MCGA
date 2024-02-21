using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IDepositosService
    {
        Task CrearDeposito(DepositosModel depositosModel);
        Task<IEnumerable<DepositosModel>> ObtenerDepositos();
    }
}
