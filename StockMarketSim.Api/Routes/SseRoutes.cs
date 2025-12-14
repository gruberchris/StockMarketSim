using StockMarketSim.Api.Services;

namespace StockMarketSim.Api.Routes;

public static class SseRoutes
{
    public static void MapSseRoutes(this WebApplication app)
    {
        var sseRoutes = app.MapGroup("/stocks");

        sseRoutes.MapGet("/live", async (HttpContext context, SseService sseService) =>
        {
            context.Response.Headers.ContentType = "text/event-stream";
            context.Response.Headers.CacheControl = "no-cache";
            context.Response.Headers.Connection = "keep-alive";
            
            // Disable response buffering to ensure immediate SSE delivery
            var bufferingFeature = context.Features.Get<Microsoft.AspNetCore.Http.Features.IHttpResponseBodyFeature>();
            bufferingFeature?.DisableBuffering();
            
            sseService.AddClient(context);

            try
            {
                // Keep the connection open until the client disconnects
                await Task.Delay(Timeout.Infinite, context.RequestAborted);
            }
            catch (TaskCanceledException)
            {
                // Client disconnected
            }
            finally
            {
                sseService.RemoveClient(context);
            }
        });
    }
}

