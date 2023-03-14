using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiarDrekiTrader.Data.Models
{
    public class StockTransaction
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public int Qty { get; set; }
        public decimal Val { get; set; }
        public decimal TotalVal { get; set; }
        public string Comments { get; set; }
    }
}
