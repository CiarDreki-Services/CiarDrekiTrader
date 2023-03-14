using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiarDrekiTrader.Data.Models
{
    public class OwnedStock
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public string Symbol { get; set; }
    }
}
