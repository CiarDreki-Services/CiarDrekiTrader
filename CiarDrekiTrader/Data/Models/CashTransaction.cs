using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiarDrekiTrader.Data.Models
{
    public class CashTransaction
    {
        public int Id { get; set; }
        public decimal Val { get; set; }
        public string Comments { get; set; }
    }
}
