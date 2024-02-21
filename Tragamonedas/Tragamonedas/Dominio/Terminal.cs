using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tragamonedas.Dominio
{
    public class Terminal
    {
        public string accion { get; set; }
        public int nroMaquina { get; set; }
        public int nroCasino { get; set; }
        public double apuesta { get; set; }
        public bool ganador { get; set; }
        public double maximo { get; set; }
        public double minimo { get; set; }
    }
}
