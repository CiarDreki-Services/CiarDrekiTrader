using CiarDrekiTrader.Data;
using CiarDrekiTrader.Helpers;

Console.WriteLine(" _____ _           ______          _    _   _____             _           \r\n/  __ (_)          |  _  \\        | |  (_) |_   _|           | |          \r\n| /  \\/_  __ _ _ __| | | |_ __ ___| | ___    | |_ __ __ _  __| | ___ _ __ \r\n| |   | |/ _` | '__| | | | '__/ _ \\ |/ / |   | | '__/ _` |/ _` |/ _ \\ '__|\r\n| \\__/\\ | (_| | |  | |/ /| | |  __/   <| |   | | | | (_| | (_| |  __/ |   \r\n \\____/_|\\__,_|_|  |___/ |_|  \\___|_|\\_\\_|   \\_/_|  \\__,_|\\__,_|\\___|_|   \r\n                                                                          \r\n                                                                          ");
Console.WriteLine("--------------------------------------------------------------------------");
Console.WriteLine("Welcome to the CiarDreki Trader.");

CiarDrekiTradingContext ciarDrekiTradingContext = new();

Console.WriteLine("Constructing variables...");
var cashManager = new CashManager(ciarDrekiTradingContext);
var topMovers = new TopMovers(ciarDrekiTradingContext);
var stockAgent = new StockAgent(ciarDrekiTradingContext);
var ownedStockAgent = new OwnedStockAgent(ciarDrekiTradingContext);


ownedStockAgent.BuildPortfolioFromTransactions();
stockAgent.GetStockAssetTotal();
stockAgent.SellOldStocks();

stockAgent.PurchaseNewMovers(cashManager.AvailableBalance());
ownedStockAgent.BuildPortfolioFromTransactions();
Console.WriteLine($"New movers purchase complete, remaining balance: {cashManager.AvailableBalance()}");

while (true)
{
    Console.WriteLine("");
    Console.WriteLine("Sleeping for 1 minute.");
    Console.WriteLine($"Time is now: {DateTime.Now}");
    Thread.Sleep(1 * 60000);

    Console.WriteLine("Checking on new top movers...");
    topMovers.FetchMovers();

    ownedStockAgent.BuildPortfolioFromTransactions();
    stockAgent.GetStockAssetTotal();
    stockAgent.SellOldStocks();
    Console.WriteLine($"Current balance: {(decimal)cashManager.AvailableBalance()}");

    stockAgent.PurchaseNewMovers(cashManager.AvailableBalance());
    ownedStockAgent.BuildPortfolioFromTransactions();
    Console.WriteLine($"New movers purchase complete, remaining balance: {(decimal)cashManager.AvailableBalance()}");
}