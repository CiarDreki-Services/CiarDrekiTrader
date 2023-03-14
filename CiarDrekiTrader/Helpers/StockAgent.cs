using CiarDrekiTrader.Data;
using CiarDrekiTrader.Data.Models;
using ServiceStack;

namespace CiarDrekiTrader.Helpers
{
    public class StockAgent
    {
        private readonly CiarDrekiTradingContext _tradingContext;
        private readonly string _yourApi;

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
            var oldStocks = _tradingContext.OwnedStocks.Where(i => i.Symbol != latestMovers.Symbol1 && i.Symbol != latestMovers.Symbol2 && i.Symbol != latestMovers.Symbol3 && i.Symbol != latestMovers.Symbol4 && i.Symbol != latestMovers.Symbol5);

            foreach(var item in oldStocks)
            {
                SellStock(GetLatestPrice(item.Symbol), item.Qty);
            }
            _tradingContext.SaveChanges();
        }

        internal void GetStockAssetTotal()
        {
            var oldStocks = _tradingContext.OwnedStocks.OrderByDescending(i => i.Qty);
            decimal totalVal = 0;
            foreach (var item in oldStocks)
            {
              totalVal += GetLatestPrice(item.Symbol).Val * item.Qty;
            }
            Console.WriteLine($"Your current assets value is: {totalVal}");
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
            List<AlphaVantageTSD> monthlyPrices = GetFromAPI(symbol);

            var stockPrice = new StockPrice()
            {
                Symbol = symbol,
                Val = monthlyPrices.FirstOrDefault().Close,
                OfficialTimeStamp = monthlyPrices.FirstOrDefault().Timestamp
            };

            var lastEntry = _tradingContext.StockPrices.Where(i => i.Symbol == symbol).OrderByDescending(i=>i.Id).FirstOrDefault();
            if(lastEntry != null && lastEntry.Val == monthlyPrices.FirstOrDefault().Close)
            {
                return lastEntry;
            }
            else
            {
                _tradingContext.Add(stockPrice);
            }
            return stockPrice;
        }

        private List<AlphaVantageTSD> GetFromAPI(string symbol)
        {
            List<AlphaVantageTSD> monthlyPrices;
            try
            {
                if(symbol.Contains("-"))
                    symbol = symbol.Substring(0, symbol.IndexOf("-"));
                monthlyPrices = string.Format("https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol={0}&apikey={1}&datatype=csv", symbol, yourApi).GetStringFromUrl().FromCsv<List<AlphaVantageTSD>>();
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
