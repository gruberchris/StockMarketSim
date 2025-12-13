# Stock Market Simulator API

A real-time stock market simulation API built with .NET 10 that streams live stock price updates using Server-Sent Events (SSE). This application demonstrates modern ASP.NET Core Minimal API features including built-in validation, background services, and real-time data streaming.

## Features

- 📊 **Real-time Stock Price Updates**: Uses Server-Sent Events (SSE) to push live price changes to connected clients
- 🔄 **Background Price Simulation**: Automated stock price fluctuations every 5 seconds
- 🛠️ **Full CRUD API**: Create, read, update, and delete stock ticker data
- ✅ **Built-in Validation**: Leverages .NET 10's native Minimal API validation
- 🚀 **Minimal API Design**: Clean, modern ASP.NET Core architecture
- 🐳 **Docker Support**: Ready-to-use containerization
- 📝 **Interactive API Documentation**: Beautiful Scalar UI for testing and exploring endpoints
- 📋 **OpenAPI 3.0 Specification**: Auto-generated API documentation via .NET 10's OpenAPI support

## Technology Stack

- **.NET 10**: Latest .NET runtime and SDK
- **ASP.NET Core Minimal API**: Lightweight, high-performance API framework
- **Server-Sent Events (SSE)**: Real-time unidirectional data streaming
- **C# 14**: Modern C# language features
- **In-Memory Storage**: Concurrent dictionary-based data store
- **Docker**: Containerization support

## Prerequisites

### For Local Development
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### For Docker
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## Getting Started

### Running Locally

1. **Clone or navigate to the project directory**:
   ```bash
   cd /path/to/StockMarketSim
   ```

2. **Restore dependencies**:
   ```bash
   cd StockMarketSim.Api
   dotnet restore
   ```

3. **Run the application**:
   ```bash
   dotnet run
   ```

4. **Access the API**:
   - API Base URL: `https://localhost:7214` or `http://localhost:5136`
   - **Interactive API Docs (Scalar UI)**: `https://localhost:7214/scalar/v1` (Development mode) ✨
   - OpenAPI JSON: `https://localhost:7214/openapi/v1.json` (Development mode)
   - SSE Live Updates: `https://localhost:7214/stocks/live`

### Running with Docker

1. **Build the Docker image** (from the solution root):
   ```bash
   docker build -t stockmarketsim-api -f StockMarketSim.Api/Dockerfile .
   ```

2. **Run the container**:
   ```bash
   docker run -d -p 8080:8080 -p 8081:8081 --name stockmarketsim stockmarketsim-api
   ```

3. **Access the API**:
   - API Base URL: `http://localhost:8080`

4. **Stop the container**:
   ```bash
   docker stop stockmarketsim
   docker rm stockmarketsim
   ```

## API Endpoints

### Stock Management

#### Get All Stocks
```http
GET /stocks
```
Returns a list of all stocks in the system.

**Response**: `200 OK`
```json
[
  {
    "tickerSymbol": "AAPL",
    "companyName": "Apple",
    "price": 150.75
  }
]
```

#### Get Stock by Ticker
```http
GET /stocks/{ticker}
```
Returns a single stock by its ticker symbol.

**Response**: `200 OK` or `404 Not Found`

#### Create Stock
```http
POST /stocks
Content-Type: application/json

{
  "tickerSymbol": "NFLX",
  "companyName": "Netflix",
  "price": 450.00
}
```

**Validation Rules**:
- `tickerSymbol`: Required, max 10 characters
- `companyName`: Required, max 100 characters
- `price`: Range 0-10000

**Response**: `201 Created`

#### Update Stock
```http
PUT /stocks/{ticker}
Content-Type: application/json

{
  "companyName": "Netflix Inc.",
  "price": 455.50
}
```

**Validation Rules**:
- `companyName`: Optional, max 100 characters
- `price`: Optional, range 0-10000

**Response**: `200 OK` or `404 Not Found`

#### Delete Stock
```http
DELETE /stocks/{ticker}
```

**Response**: `204 No Content`

### Real-time Stock Updates

#### Subscribe to Live Stock Price Updates
```http
GET /stocks/live
```

This endpoint establishes a Server-Sent Events (SSE) connection and streams real-time stock price updates.

**Response**: `text/event-stream`
```
data: {"TickerSymbol":"AAPL","CompanyName":"Apple","Price":151.23}

data: {"TickerSymbol":"MSFT","CompanyName":"Microsoft","Price":299.87}
```

**Example using curl**:
```bash
curl -N http://localhost:5000/stocks/live
```

**Example using JavaScript**:
```javascript
const eventSource = new EventSource('http://localhost:5000/stocks/live');

eventSource.onmessage = (event) => {
  const stock = JSON.parse(event.data);
  console.log(`${stock.TickerSymbol}: $${stock.Price}`);
};

eventSource.onerror = (error) => {
  console.error('SSE Error:', error);
  eventSource.close();
};
```

### API Documentation

#### Interactive API Documentation (Scalar UI)
```http
GET /scalar/v1
```

Access a beautiful, interactive API documentation interface powered by Scalar. This endpoint is only available in Development mode.

**Features**:
- 🎨 Modern, clean interface for browsing all API endpoints
- 🧪 Test API endpoints directly in the browser
- 📖 Auto-generated documentation from your code
- 🔍 Search and filter capabilities
- 📝 Request/response examples with validation rules

**Access in browser**:
```bash
open https://localhost:7214/scalar/v1
```

**Screenshot**: The Scalar UI displays all CRUD endpoints, DTOs, validation rules, and allows you to execute requests with a built-in API client.

#### OpenAPI Specification (JSON)
```http
GET /openapi/v1.json
```

Retrieve the raw OpenAPI 3.0 specification document in JSON format. This endpoint is only available in Development mode.

**Use cases**:
- Import into API clients (Postman, Insomnia, etc.)
- Generate client SDKs for various programming languages
- Integrate with API management tools
- View the complete API contract

**Example using curl**:
```bash
curl -k https://localhost:7214/openapi/v1.json | jq
```

**Example response structure**:
```json
{
  "openapi": "3.0.1",
  "info": {
    "title": "StockMarketSim.Api",
    "version": "1.0"
  },
  "paths": {
    "/stocks": { ... },
    "/stocks/{ticker}": { ... },
    "/stocks/live": { ... }
  },
  "components": {
    "schemas": {
      "Stock": { ... },
      "CreateStockDto": { ... },
      "UpdateStockDto": { ... }
    }
  }
}
```

**Helper Scripts**:

A helper script is provided for easy access to API documentation:
```bash
./view-openapi.sh
```

Choose from:
- **Option 1**: Open Scalar UI in browser (recommended)
- **Option 2**: Open OpenAPI JSON specification in browser
- **Option 3**: View formatted specification in terminal

**Note**: Both OpenAPI endpoints are only available when running in Development mode (default for local development).

## Configuration

The application can be configured via `appsettings.json`:

### Stock Simulation Settings
```json
{
  "StockSettings": {
    "UpdateIntervalSeconds": 5
  }
}
```

### Initial Stock Data
The application seeds with 10 predefined stocks on startup. You can modify the `InitialStocks` section in `appsettings.json`:

```json
{
  "InitialStocks": [
    {
      "TickerSymbol": "AAPL",
      "CompanyName": "Apple",
      "Price": 150.00
    }
  ]
}
```

## Project Structure

```
StockMarketSim.Api/
├── Data/
│   └── StockStore.cs              # In-memory stock data storage
├── Models/
│   ├── Stock.cs                   # Stock entity model
│   ├── StockSettings.cs           # Configuration model
│   └── Dtos/
│       ├── CreateStockDto.cs      # DTO for creating stocks
│       └── UpdateStockDto.cs      # DTO for updating stocks
├── Routes/
│   └── StockRoutes.cs             # Stock endpoint handlers
├── Services/
│   ├── SseService.cs              # Server-Sent Events connection manager
│   └── StockPriceSimulator.cs     # Background price update service
├── Program.cs                     # Application entry point
├── appsettings.json               # Application configuration
└── Dockerfile                     # Docker container definition
```

## How It Works

1. **Startup**: The application loads initial stock data from `appsettings.json` into an in-memory store
2. **Background Service**: `StockPriceSimulator` runs continuously, updating random stock prices every 5 seconds
3. **SSE Broadcasting**: When prices update, all connected SSE clients receive the updated stock data in real-time
4. **CRUD Operations**: Standard REST endpoints allow managing the stock data that the simulator uses

## C# 14 Features

This project explicitly targets **C# 14** and leverages the new `field` keyword feature for enhanced property validation and normalization.

### The `field` Keyword

C# 14 introduces the `field` keyword, which provides direct access to the compiler-generated backing field within property accessors. This eliminates the need for manually declared backing fields while enabling inline validation, transformation, and business logic.

#### Example: Price Validation in `Stock.cs`

```csharp
public decimal Price
{
    get => field;
    set => field = Math.Round(Math.Max(0, value), 2);
}
```

This property automatically:
- Clamps negative values to 0
- Rounds to 2 decimal places
- Uses the compiler-generated backing field via `field` keyword

#### Example: Configuration Validation in `StockSettings.cs`

```csharp
public required int UpdateIntervalSeconds
{
    get => field;
    init
    {
        if (value < 1)
        {
            throw new ArgumentException("Update interval must be at least 1 second.", nameof(UpdateIntervalSeconds));
        }
        field = value;
    }
}
```

This property ensures the update interval is always at least 1 second by throwing an exception for invalid values.

#### Example: String Normalization in `CreateStockDto.cs`

```csharp
public required string TickerSymbol
{
    get => field;
    init => field = value?.Trim().ToUpperInvariant() ?? string.Empty;
}
```

This property automatically trims whitespace and converts ticker symbols to uppercase for consistency.

### Benefits in This Project

1. **Automatic Price Rounding**: All price assignments are automatically rounded to 2 decimals
2. **Input Sanitization**: Ticker symbols are uppercased, company names are trimmed
3. **Business Rule Enforcement**: Prices cannot be negative, intervals must be positive
4. **Cleaner Code**: No manual backing fields needed, validation logic is centralized in properties
5. **Type Safety**: Compiler-generated backing fields with full type inference

## Development

### Building the Project
```bash
dotnet build
```

### Running Tests (if implemented)
```bash
dotnet test
```

### Cleaning Build Artifacts
```bash
dotnet clean
```

## License

This is a demonstration project for educational purposes.

## Contributing

This is a sample project. Feel free to fork and modify for your own learning purposes.

