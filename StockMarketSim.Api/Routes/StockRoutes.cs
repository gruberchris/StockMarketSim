using Microsoft.AspNetCore.Mvc;
using StockMarketSim.Api.Data;
using StockMarketSim.Api.Models;
using StockMarketSim.Api.Models.Dtos;

namespace StockMarketSim.Api.Routes;

public static class StockRoutes
{
    public static void MapStockRoutes(this WebApplication app)
    {
        var stockRoutes = app.MapGroup("/stocks");

        stockRoutes.MapGet("/", (StockStore store) => Results.Ok(store.GetAllStocks()));

        stockRoutes.MapGet("/{ticker}", (StockStore store, string ticker) =>
        {
            var stock = store.GetStockByTicker(ticker);
            return stock is not null ? Results.Ok(stock) : Results.NotFound();
        });

        stockRoutes.MapPost("/", (StockStore store, [FromBody] CreateStockDto newStock) =>
        {
            var stock = new Stock
            {
                TickerSymbol = newStock.TickerSymbol,
                CompanyName = newStock.CompanyName,
                Price = newStock.Price
            };
            store.AddStock(stock);
            return Results.Created($"/stocks/{stock.TickerSymbol}", stock);
        });

        stockRoutes.MapPut("/{ticker}", (StockStore store, string ticker, [FromBody] UpdateStockDto updatedStock) =>
        {
            var existingStock = store.GetStockByTicker(ticker);
            if (existingStock is null)
            {
                return Results.NotFound();
            }

            existingStock.CompanyName = updatedStock.CompanyName ?? existingStock.CompanyName;
            existingStock.Price = updatedStock.Price ?? existingStock.Price;

            store.UpdateStock(ticker, existingStock);
            return Results.Ok(existingStock);
        });

        stockRoutes.MapDelete("/{ticker}", (StockStore store, string ticker) =>
        {
            store.DeleteStock(ticker);
            return Results.NoContent();
        });
    }
}

