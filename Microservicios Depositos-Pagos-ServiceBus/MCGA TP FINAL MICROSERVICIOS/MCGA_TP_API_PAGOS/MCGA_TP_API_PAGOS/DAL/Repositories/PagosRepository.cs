using DAL.Interfaces;
using Domain.AppSettings;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class PagosRepository : GenericRepository<PagosModel>, IPagosRepository 
    {
        public PagosRepository(ConnectionStrings connectionString) : base(connectionString)
        {
        }
    }
}
