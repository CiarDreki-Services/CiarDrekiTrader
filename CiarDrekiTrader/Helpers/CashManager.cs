using CiarDrekiTrader.Data;
using Microsoft.Extensions.Logging;

namespace CiarDrekiTrader.Helpers
{
    public class CashManager
    {
        private readonly CiarDrekiTradingContext _tradingContext;
        private readonly decimal currentMinimum = 100;

        public CashManager(CiarDrekiTradingContext tradingContext)
        {
            _tradingContext = tradingContext;
            CheckStatus();
        }
        public void CheckStatus()
        {
            if (_tradingContext.CashTransactions.ToList().Count == 0)
            {
                Console.WriteLine($"No transactions found, a founding amount of ${currentMinimum} is needed to begin.");

            }
            else
            {
                Console.WriteLine($"Available Balance: {_tradingContext.CashTransactions.Sum(i => i.Val)}");
            }
        }
        public decimal AvailableBalance()
        {
            return _tradingContext.CashTransactions.Sum(i => i.Val);
        }

    }
}
