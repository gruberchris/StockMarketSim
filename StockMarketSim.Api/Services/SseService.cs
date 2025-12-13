using System.Collections.Concurrent;
using System.Text;

namespace StockMarketSim.Api.Services;

public class SseService
{
    private readonly ConcurrentDictionary<string, HttpContext> _clients = new();

    public void AddClient(HttpContext context)
    {
        _clients.TryAdd(context.Connection.Id, context);
    }

    public void RemoveClient(HttpContext context)
    {
        _clients.TryRemove(context.Connection.Id, out _);
    }

    public async Task SendToAllClientsAsync(string data)
    {
        var payload = $"data: {data}\n\n";
        var payloadBytes = Encoding.UTF8.GetBytes(payload);

        foreach (var client in _clients.Values)
        {
            try
            {
                await client.Response.Body.WriteAsync(payloadBytes);
                await client.Response.Body.FlushAsync();
            }
            catch (Exception)
            {
                // Client disconnected
            }
        }
    }
}

