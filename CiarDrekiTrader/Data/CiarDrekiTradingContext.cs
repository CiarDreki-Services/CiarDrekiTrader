using CiarDrekiTrader.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CiarDrekiTrader.Data
{
    public class CiarDrekiTradingContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=CiarDrekiTrader;MultipleActiveResultSets=true");
        }

        public DbSet<TopStocks> TopStocks { get; set; }
        public DbSet<CashTransaction> CashTransactions { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }
        public DbSet<StockPrice> StockPrices { get; set; }
        public DbSet<OwnedStock> OwnedStocks { get; set; }
    }
}
