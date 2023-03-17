using CiarDrekiTrader.Data;
using CiarDrekiTrader.Data.Models;
using Newtonsoft.Json;
using ServiceStack;
using System.Net;
using static System.Net.WebRequestMethods;

namespace CiarDrekiTrader.Helpers
{
    public class StockAgent
    {
        private readonly CiarDrekiTradingContext _tradingContext;
        private readonly string _yourApi;
        private List<StockPrice> stockPrices = new();
        private readonly string url = "https://query1.finance.yahoo.com/v8/finance/chart/{0}?region=US&lang=en-US";


        public StockAgent(CiarDrekiTradingContext tradingContext, string yourAPI)
        {
            _tradingContext = tradingContext;
            _yourApi = yourAPI;
        }

        public void PurchaseNewMovers(decimal availableBalance)
        {
            availableBalance = availableBalance / 5;
            var latestMovers = _tradingContext.TopStocks.OrderByDescending(i => i.Id).FirstOrDefault();

            var symbol1Sp = GetLatestPrice(latestMovers.Symbol1);
            var symbol2Sp = GetLatestPrice(latestMovers.Symbol2);
            var symbol3Sp = GetLatestPrice(latestMovers.Symbol3);
            var symbol4Sp = GetLatestPrice(latestMovers.Symbol4);
            var symbol5Sp = GetLatestPrice(latestMovers.Symbol5);

            PurchaseMaxStock(availableBalance, symbol1Sp);
            PurchaseMaxStock(availableBalance, symbol2Sp);
            PurchaseMaxStock(availableBalance, symbol3Sp);
            PurchaseMaxStock(availableBalance, symbol4Sp);
            PurchaseMaxStock(availableBalance, symbol5Sp);

            _tradingContext.SaveChanges();

        }

        internal void SellOldStocks()
        {
            Console.WriteLine("Checking old stock...");
            var latestMovers = _tradingContext.TopStocks.OrderByDescending(i => i.Id).FirstOrDefault();
            var oldStocks = _tradingContext.OwnedStocks.OrderByDescending(i => i.Qty);

            decimal totalVal = 0;
            foreach (var item in oldStocks)
            {
                var stock = GetLatestPrice(item.Symbol);
                stockPrices.Add(stock);
                totalVal += stock.Val * item.Qty;
            }
            Console.WriteLine($"Your current assets value is: {totalVal}");
            var stockToSell = oldStocks.Where(i => i.Symbol != latestMovers.Symbol1 && i.Symbol != latestMovers.Symbol2 && i.Symbol != latestMovers.Symbol3 && i.Symbol != latestMovers.Symbol4 && i.Symbol != latestMovers.Symbol5);
            foreach (var item in stockToSell)
            {
                SellStock(stockPrices.Find(i => i.Symbol == item.Symbol), item.Qty);
            }
            _tradingContext.SaveChanges();
        }

        private void SellStock(StockPrice stock, int qty, string comments = null)
        {
            var stx = new StockTransaction()
            {
                Symbol = stock.Symbol,
                Qty = qty * -1,
                Val = stock.Val,
                TotalVal = qty * stock.Val,
                Comments = string.IsNullOrEmpty(comments) ? "Sold as part of Old Stocks" : comments
            };
            _tradingContext.Add(stx);

            var ctx = new CashTransaction()
            {
                Val = stx.TotalVal,
                Comments = $"Sold {stx.Qty} of stock: {stx.Symbol} for: ${stx.Val}. Total price: ${stx.TotalVal}"
            };
            _tradingContext.Add(ctx);
            Console.WriteLine(ctx.Comments);
        }
    
        private StockPrice GetLatestPrice(string symbol)
        {
            var monthlyPrices = GetFromAPI(symbol);

            var stockPrice = new StockPrice()
            {
                Symbol = symbol,
                Val = monthlyPrices.Close,
                OfficialTimeStamp = monthlyPrices.Timestamp
            };

            var lastEntry = _tradingContext.StockPrices.Where(i => i.Symbol == symbol).OrderByDescending(i=>i.Id).FirstOrDefault();
            if(lastEntry != null && lastEntry.Val == stockPrice.Val)
            {
                return lastEntry;
            }
            else
            {
                _tradingContext.Add(stockPrice);
            }
            return stockPrice;
        }

        private YahooFinance GetFromAPI(string symbol)
        {
            YahooFinance monthlyPrices;
            try
            {
                if(symbol.Contains("-"))
                    symbol = symbol.Substring(0, symbol.IndexOf("-"));
                string jsonString;
                using(var client = new WebClient())
                {
                    jsonString = client.DownloadString(string.Format("https://query1.finance.yahoo.com/v8/finance/chart/{0}?region=US&lang=en-US", symbol));
                }
                var response = JsonConvert.DeserializeObject<Root>(jsonString);
                monthlyPrices = new YahooFinance()
                {
                    Timestamp = new DateTime(1970,01,01).AddSeconds(response.Chart.Result.FirstOrDefault().Timestamp.FirstOrDefault()),
                    Open = response.Chart.Result.FirstOrDefault().Indicators.Quote.FirstOrDefault().Open.FirstOrDefault().Value,
                    High = response.Chart.Result.FirstOrDefault().Indicators.Quote.FirstOrDefault().High.FirstOrDefault().Value,
                    Low = response.Chart.Result.FirstOrDefault().Indicators.Quote.FirstOrDefault().Low.FirstOrDefault().Value,
                    Close = response.Chart.Result.FirstOrDefault().Indicators.Quote.FirstOrDefault().Close.FirstOrDefault().Value,
                    Volume = response.Chart.Result.FirstOrDefault().Indicators.Quote.FirstOrDefault().Volume.FirstOrDefault().Value
                };
                //monthlyPrices = string.Format("https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol={0}&apikey={1}&datatype=csv", symbol, _yourApi).GetStringFromUrl().FromCsv<List<AlphaVantageTSD>>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed \nRetrying in 60 seconds...");
                Thread.Sleep(60000);
                Console.WriteLine("Retrying...");
                monthlyPrices = GetFromAPI(symbol);
            }
            return monthlyPrices;
        }

        private void PurchaseMaxStock(decimal availableBalance, StockPrice stock)
        {
            int maxCanPurchase = (int)Math.Floor(availableBalance / stock.Val);
            if(maxCanPurchase > 0)
            {
                PurchaseStock(maxCanPurchase, stock);
            }
        }

        private void PurchaseStock(int qty, StockPrice stock, string comments = null)
        {
            var stx = new StockTransaction()
            {
                Symbol = stock.Symbol,
                Qty = qty,
                Val = stock.Val *-1,
                TotalVal = qty * (stock.Val *-1),
                Comments = string.IsNullOrEmpty(comments) ? "Purchased as part of New Movers" : comments
            };
            _tradingContext.Add(stx);

            var ctx = new CashTransaction()
            {
                Val = stx.TotalVal,
                Comments = $"Purchased {stx.Qty} of stock: {stx.Symbol} for: ${stx.Val}. Total price: ${stx.TotalVal}"
            };
            _tradingContext.Add(ctx);

            Console.WriteLine(ctx.Comments);
        }
    }
}
