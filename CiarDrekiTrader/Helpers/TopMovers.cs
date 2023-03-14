using CiarDrekiTrader.Data;
using CiarDrekiTrader.Data.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace CiarDrekiTrader.Helpers
{

    public class TopMovers
    {
        private readonly CiarDrekiTradingContext _tradingContext;
        private readonly string _url;
        private List<TopMoversObject> _movers;

        public TopMovers(CiarDrekiTradingContext tradingContext)
        {
            _tradingContext = tradingContext;
            _url = "https://financialmodelingprep.com/api/v3/stock_market/gainers?apikey=ca6926269fdc54230bf39289c069a61f";
            FetchMovers();
        }

        public List<TopMoversObject> Top5
        {
            get
            {
                return _movers;
            }
        }

        public void FetchMovers()
        {
            string jsonString;
            using (var wc = new WebClient())
            {
                jsonString = wc.DownloadString(_url);
            }
            //jsonString = jsonString.Split(new string[] { "\"results\":{\"rows\":" }, StringSplitOptions.None)[1];
            //jsonString = jsonString.Split(new string[] { "]" }, StringSplitOptions.None)[0] + "]";

            List<TopMoversObject> results = JsonConvert.DeserializeObject<List<TopMoversObject>>(jsonString);
            _movers = new List<TopMoversObject>();
            _movers.AddRange(results.Where(i=>i.Symbol != "ZJZZT" && i.Symbol != "ZWZZT").Take(5));

            Console.WriteLine("Top movers discovered: ");
            for(int i = 0; i < _movers.Count; i++)
            {
                Console.WriteLine($"{i} - \t{_movers[i].Symbol} \t{_movers[i].ChangesPercentage}");
            }
            CommitToDb();
        }

        private void CommitToDb()
        {
            var lastEntry = _tradingContext.TopStocks.OrderByDescending(i=>i.Id).FirstOrDefault();
            if (lastEntry != null && lastEntry.Symbol1 == _movers[0].Symbol && lastEntry.Symbol2 == _movers[1].Symbol && lastEntry.Symbol3 == _movers[2].Symbol && lastEntry.Symbol4 == _movers[3].Symbol && lastEntry.Symbol5 == _movers[4].Symbol)
            {
                Console.WriteLine($"Top stocks Not Committed to DB, same entry as before.");
            }
            else
            {
                _tradingContext.Add(new TopStocks()
                {
                    Generated = DateTime.Now,
                    Symbol1 = _movers[0].Symbol,
                    PcChange1 = (decimal)_movers[0].ChangesPercentage,
                    Symbol2 = _movers[1].Symbol,
                    PcChange2 = (decimal)_movers[1].ChangesPercentage,
                    Symbol3 = _movers[2].Symbol,
                    PcChange3 = (decimal)_movers[2].ChangesPercentage,
                    Symbol4 = _movers[3].Symbol,
                    PcChange4 = (decimal)_movers[3].ChangesPercentage,
                    Symbol5 = _movers[4].Symbol,
                    PcChange5 = (decimal)_movers[4].ChangesPercentage,
                });
                _tradingContext.SaveChanges();
                Console.WriteLine($"{_movers[0].Symbol}, {_movers[1].Symbol}, {_movers[2].Symbol}, {_movers[3].Symbol}, and {_movers[4].Symbol} Added as new movers.");
            }
        }
    }
}
