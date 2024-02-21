using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class PagosException : Exception
    {
        public PagosException(string message) : base(message)
        {
                
        }
    }
}
