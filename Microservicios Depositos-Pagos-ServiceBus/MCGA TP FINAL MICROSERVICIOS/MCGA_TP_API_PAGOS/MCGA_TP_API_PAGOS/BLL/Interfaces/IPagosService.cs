﻿using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IPagosService
    {
        Task CrearPago(PagosModel pagosModel);
        Task<IEnumerable<PagosModel>> ObtenerPagos();
    }
}
