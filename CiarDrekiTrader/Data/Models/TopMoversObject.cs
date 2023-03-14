using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiarDrekiTrader.Data.Models
{
    public class TopMoversObject
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Change { get; set; }
        public decimal Price { get; set; }
        public decimal ChangesPercentage { get; set; }
    }
}
