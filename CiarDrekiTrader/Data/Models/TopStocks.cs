using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiarDrekiTrader.Data.Models
{
    public class TopStocks
    {
        public int Id { get; set; }
        public DateTime Generated { get; set; }
        public string Symbol1 { get; set; }
        public decimal PcChange1 { get; set; }
        public string Symbol2 { get; set; }
        public decimal PcChange2 { get; set; }
        public string Symbol3 { get; set; }
        public decimal PcChange3 { get; set; }
        public string Symbol4 { get; set; }
        public decimal PcChange4 { get; set; }
        public string Symbol5 { get; set; }
        public decimal PcChange5 { get; set; }
    }
}
