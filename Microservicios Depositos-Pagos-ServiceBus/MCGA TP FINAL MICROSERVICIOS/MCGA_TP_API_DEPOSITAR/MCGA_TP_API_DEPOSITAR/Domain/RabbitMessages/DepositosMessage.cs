using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RabbitMessages
{
    public class DepositosMessage
    {
        public int idterminal { get; set; }
        public int idcasino { get; set; }
        public decimal monto { get; set; }
    }
}
