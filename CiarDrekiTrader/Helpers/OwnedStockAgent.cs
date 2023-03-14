using CiarDrekiTrader.Data;
using CiarDrekiTrader.Data.Models;
using Microsoft.Extensions.Logging;

namespace CiarDrekiTrader.Helpers
{
    public class OwnedStockAgent
    {
        private readonly CiarDrekiTradingContext _tradingContext;

        public OwnedStockAgent(CiarDrekiTradingContext tradingContext)
        {
            _tradingContext = tradingContext;
        }

        public void BuildPortfolioFromTransactions()
        {
            _tradingContext.OwnedStocks.RemoveRange(_tradingContext.OwnedStocks);
            _tradingContext.SaveChanges();

            var transactions = _tradingContext.StockTransactions.ToList();
            foreach ( var transaction in transactions )
            {
                UpdateStock(transaction.Symbol, transaction.Qty);
            }

            DeleteZeroOwnedStocks();
        }

        private void UpdateStock(string symbol, int diff) 
        {
            var stockItem = _tradingContext.OwnedStocks.Where(i=>i.Symbol == symbol).FirstOrDefault();
            if(stockItem == null) 
            {
                _tradingContext.OwnedStocks.Add(new OwnedStock
                {
                    Qty = diff,
                    Symbol = symbol
                });
            }
            else
            {
                stockItem.Qty += diff;
            }
            _tradingContext.SaveChanges();
        }

        public void DeleteZeroOwnedStocks() 
        {
            _tradingContext.OwnedStocks.RemoveRange(_tradingContext.OwnedStocks.Where(i => i.Qty < 1));
            _tradingContext.SaveChanges();
        }
    }
}
