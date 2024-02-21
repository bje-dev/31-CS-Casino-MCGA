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
    public class DepositosRepository : GenericRepository<DepositosModel>, IDepositosRepository 
    {
        public DepositosRepository(ConnectionStrings connectionString) : base(connectionString)
        {
        }
    }
}
