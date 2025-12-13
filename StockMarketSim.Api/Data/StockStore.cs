using System.Collections.Concurrent;
using StockMarketSim.Api.Models;

namespace StockMarketSim.Api.Data;

public class StockStore(IConfiguration configuration)
{
    private readonly ConcurrentDictionary<string, Stock> _stocks = new(
        configuration.GetSection("InitialStocks")
            .Get<List<Stock>>()
            ?.ToDictionary(s => s.TickerSymbol, s => s) 
            ?? []);

    public IEnumerable<Stock> GetAllStocks() => _stocks.Values;

    public Stock? GetStockByTicker(string ticker) => _stocks.GetValueOrDefault(ticker);

    public bool AddStock(Stock stock) => _stocks.TryAdd(stock.TickerSymbol, stock);

    public void UpdateStock(string ticker, Stock stock) => _stocks[ticker] = stock;

    public bool DeleteStock(string ticker) => _stocks.TryRemove(ticker, out _);
}


