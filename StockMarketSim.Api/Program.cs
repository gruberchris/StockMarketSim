using Scalar.AspNetCore;
using StockMarketSim.Api.Data;
using StockMarketSim.Api.Models;
using StockMarketSim.Api.Routes;
using StockMarketSim.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<StockStore>();
builder.Services.AddSingleton<SseService>();
builder.Services.AddHostedService<StockPriceSimulator>();
builder.Services.Configure<StockSettings>(builder.Configuration.GetSection("StockSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapStockRoutes();
app.MapSseRoutes();

app.Run();
