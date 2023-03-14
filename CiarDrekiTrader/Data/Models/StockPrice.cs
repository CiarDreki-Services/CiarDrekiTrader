using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiarDrekiTrader.Data.Models
{
    public class StockPrice
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public decimal Val { get; set; }
        public DateTime OfficialTimeStamp { get; set; }
    }
}
