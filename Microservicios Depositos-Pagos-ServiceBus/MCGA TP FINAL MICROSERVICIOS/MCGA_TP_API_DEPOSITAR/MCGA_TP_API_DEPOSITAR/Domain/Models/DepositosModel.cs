using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    [Table("Apuestas")]
    public class DepositosModel
    {
        [Key]
        [Column("id")]
        public int IdDeposito { get; set; }

        [Column("id_terminal")]
        public int IdTerminal { get; set; }

        [Column("id_casino")]
        public int IdCasino { get; set; }

        [Column("monto")]
        public decimal Monto { get; set; }

        [Column("fecha")]
        public DateTime FechaPago { get; set; }
    }
}
