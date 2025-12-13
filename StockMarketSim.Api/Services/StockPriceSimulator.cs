using System.Text.Json;
using Microsoft.Extensions.Options;
using StockMarketSim.Api.Data;
using StockMarketSim.Api.Models;


namespace StockMarketSim.Api.Services;

public class StockPriceSimulator(IServiceProvider serviceProvider, IOptions<StockSettings> stockSettings)
    : IHostedService, IDisposable
{
    private readonly StockSettings _stockSettings = stockSettings.Value;
    private Timer? _timer;
    private readonly Random _random = new();

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(_stockSettings.UpdateIntervalSeconds));
        return Task.CompletedTask;
    }

    private async void TimerCallback(object? state)
    {
        try
        {
            await UpdateStockPrices();
        }
        catch (Exception)
        {
            // Log exception if needed - swallow to prevent a process crash
        }
    }

    private async Task UpdateStockPrices()
    {
        using var scope = serviceProvider.CreateScope();
        var stockStore = scope.ServiceProvider.GetRequiredService<StockStore>();
        var sseService = scope.ServiceProvider.GetRequiredService<SseService>();

        var stocks = stockStore.GetAllStocks().ToList();
        if (stocks.Count == 0) return;

        var stockToUpdate = stocks[_random.Next(stocks.Count)];
        var priceChange = (decimal)(_random.NextDouble() * 2 - 1); // -1 to +1
        stockToUpdate.Price += priceChange; // Price property handles rounding and clamping

        stockStore.UpdateStock(stockToUpdate.TickerSymbol, stockToUpdate);

        var json = JsonSerializer.Serialize(stockToUpdate);
        await sseService.SendToAllClientsAsync(json);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
        GC.SuppressFinalize(this);
    }
}

