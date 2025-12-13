# Stock Market Simulator API Implementation Plan

This document outlines the steps to build a real-time stock market simulator using a .NET 10 Minimal API and Server-Sent Events (SSE).

## Step 1: Project Setup and Cleanup

1.  **Clean `Program.cs`**: Remove the existing `/weatherforecast` endpoint and related code.
2.  **Install Packages**: Add necessary NuGet packages for validation if not already present.

## Step 2: Data Models and Validation

1.  **Create `Stock.cs`**: Define a `Stock` record with properties for `TickerSymbol`, `CompanyName`, and `Price`.
2.  **Create `StockDto.cs`**: Define DTOs for creating and updating stocks.
3.  **Add Validation**: Use data annotations (`[Required]`, `[StringLength]`, etc.) on the DTOs to enforce validation rules.

## Step 3: In-Memory Data Store

1.  **Create `StockStore.cs`**: An in-memory store to manage the collection of stocks.
2.  **Implement CRUD Logic**:
    *   `GetAllStocks()`: Returns all stocks.
    *   `GetStockByTicker(string ticker)`: Returns a single stock.
    *   `AddStock(Stock stock)`: Adds a new stock.
    *   `UpdateStock(Stock stock)`: Updates an existing stock.
    *   `DeleteStock(string ticker)`: Deletes a stock.
3.  **Seed Data**: Load an initial list of 10 stocks from `appsettings.json` at startup.

## Step 4: API Route Handlers

1.  **Create `StockRoutes.cs`**: A static class to define and map the API endpoints.
2.  **Implement Route Handlers**: Create methods that will handle the HTTP requests and call the appropriate methods in `StockStore`.
    *   `MapGet("/stocks", ...)`
    *   `MapGet("/stocks/{ticker}", ...)`
    *   `MapPost("/stocks", ...)`
    *   `MapPut("/stocks/{ticker}", ...)`
    *   `MapDelete("/stocks/{ticker}", ...)`
3.  **Enable Validation**: Ensure the endpoints that accept DTOs validate the incoming data and return appropriate error responses.

## Step 5: Real-time Updates with SSE

1.  **Create `SseService.cs`**: A singleton service to manage active SSE client connections (`HttpContext`).
    *   `AddClient(HttpContext context)`
    *   `RemoveClient(HttpContext context)`
    *   `SendToAllClientsAsync(string data)`
2.  **Create SSE Endpoint**:
    *   Map a GET endpoint at `/stocks/live`.
    *   When a client connects, add their `HttpContext` to the `SseService`.
    *   Keep the connection open and wait for updates. Remove the client on disconnect.
3.  **Create `StockPriceSimulator.cs`**: A background service (`IHostedService`) that runs in the background.
    *   Periodically (e.g., every 5 seconds), it will randomly update the prices of the stocks in the `StockStore`.
    *   After updating prices, it will use the `SseService` to send the updated stock data (as JSON) to all connected clients.

## Step 6: Configuration

1.  **Update `appsettings.json`**:
    *   Add a `StockSettings` section to define the simulation update interval.
    *   Add a `InitialStocks` section with a list of 10 stock objects to seed the application.
2.  **Create `StockSettings.cs`**: A class to bind the `StockSettings` configuration.

## Step 7: Dependency Injection

1.  **Register Services in `Program.cs`**:
    *   Register `StockStore` as a singleton.
    *   Register `SseService` as a singleton.
    *   Register `StockPriceSimulator` as a hosted service.
    *   Register `StockSettings` from the configuration.
2.  **Map Routes**: Call the method in `StockRoutes.cs` to map all the stock management endpoints.

