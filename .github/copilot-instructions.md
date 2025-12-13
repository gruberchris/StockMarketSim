# GitHub Copilot Instructions for Stock Market Simulator API

## Project Overview

A real-time stock market simulation API built with .NET 10 that streams live stock price updates using Server-Sent Events (SSE). This application demonstrates modern ASP.NET Core Minimal API features including built-in validation, background services, and real-time data streaming.

**Technology Stack:**
- .NET 10 (latest runtime and SDK)
- ASP.NET Core Minimal API
- C# 14 with latest language features
- Server-Sent Events (SSE) for real-time streaming
- Scalar.AspNetCore (v2.11.6) for interactive API documentation
- Microsoft.AspNetCore.OpenApi (v10.0.1) for OpenAPI 3.0 spec generation
- In-memory storage using ConcurrentDictionary

**Project Type:** Web API with real-time streaming capabilities

## Architecture and Design Patterns

This is a single-project ASP.NET Core Web API built using the Minimal API pattern (no controllers).

**Key Architectural Decisions:**
- **Minimal API Pattern**: All endpoints defined using route handlers, not controllers
- **Dependency Injection**: Extensive use of DI for all services
- **Background Services**: Uses IHostedService for continuous price simulation
- **Singleton Services**: StockStore and SseService are registered as singletons for state management
- **In-Memory Storage**: ConcurrentDictionary for thread-safe stock data storage
- **SSE for Real-Time Updates**: Server-Sent Events for unidirectional streaming from server to client
- **Primary Constructor Injection**: Modern C# pattern for dependency injection in classes

**Directory Structure:**
```
StockMarketSim.Api/
├── Data/                   # In-memory data storage
│   └── StockStore.cs       # ConcurrentDictionary-based stock repository
├── Models/                 # Domain models and configuration
│   ├── Stock.cs            # Main stock entity with C# 14 field keyword
│   ├── StockSettings.cs    # Configuration model with validation
│   └── Dtos/               # Data Transfer Objects
│       ├── CreateStockDto.cs   # Input DTO for creating stocks
│       └── UpdateStockDto.cs   # Input DTO for updating stocks
├── Routes/                 # API endpoint definitions
│   └── StockRoutes.cs      # Stock CRUD endpoints using MapGroup
├── Services/               # Business logic and background services
│   ├── SseService.cs       # Server-Sent Events client management
│   └── StockPriceSimulator.cs  # Background service for price updates
├── Program.cs              # Application entry point and configuration
├── appsettings.json        # Configuration including initial stock data
└── Dockerfile              # Multi-stage Docker build
```

## Coding Conventions

### General Guidelines

- **Language:** C# 14 (explicitly set with `<LangVersion>14</LangVersion>`)
- **Target Framework:** net10.0
- **Nullable Reference Types:** Enabled (`<Nullable>enable</Nullable>`)
- **Implicit Usings:** Enabled (`<ImplicitUsings>enable</ImplicitUsings>`)
- **Style Guide:** Follow standard .NET coding conventions
- **Formatting:** Standard C# formatting (4 spaces for indentation)

### Naming Conventions

- **Files:** PascalCase matching the primary type name (e.g., `StockStore.cs`, `SseService.cs`)
- **Namespaces:** Match directory structure (e.g., `StockMarketSim.Api.Models.Dtos`)
- **Variables:** camelCase with underscore prefix for private fields (e.g., `_stocks`, `_clients`)
- **Methods:** PascalCase (e.g., `GetAllStocks()`, `AddClient()`)
- **Classes:** PascalCase (e.g., `StockStore`, `SseService`)
- **Constants/Static Fields:** PascalCase (e.g., `UpdateIntervalSeconds`)
- **Parameters:** camelCase (e.g., `ticker`, `context`)
- **Properties:** PascalCase (e.g., `TickerSymbol`, `CompanyName`)

### Code Organization

- **Route Definitions**: Isolated in static extension method classes under `Routes/` folder
- **Services**: Business logic and infrastructure services in `Services/` folder
- **Models**: Domain entities in `Models/`, DTOs in `Models/Dtos/` subfolder
- **Data Access**: Data store implementations in `Data/` folder
- **Dependency Registration**: All service registrations in `Program.cs` before `app.Build()`
- **Route Mapping**: Called after `app.Build()` in `Program.cs`

### Language-Specific Guidelines

**C# 14 `field` Keyword - Primary Pattern:**
This project extensively uses the C# 14 `field` keyword for property backing fields with custom logic.

**Preferred Patterns:**

```csharp
// ✅ PREFERRED: Use field keyword for properties with validation/normalization
public required string CompanyName
{
    get => field;
    set
    {
        var trimmed = value?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(trimmed))
        {
            throw new ArgumentException("Company name cannot be empty or whitespace.", nameof(CompanyName));
        }
        field = trimmed;
    }
}

// ✅ PREFERRED: Use field keyword with init for DTOs
public required string TickerSymbol
{
    get => field;
    init => field = value?.Trim().ToUpperInvariant() ?? string.Empty;
}

// ✅ PREFERRED: Use field keyword for value transformation
public decimal Price
{
    get => field;
    set => field = Math.Round(Math.Max(0, value), 2);
}
```

**Primary Constructors:**

```csharp
// ✅ PREFERRED: Use primary constructors for dependency injection
public class StockStore(IConfiguration configuration)
{
    private readonly ConcurrentDictionary<string, Stock> _stocks = new(/* ... */);
}

public class StockPriceSimulator(IServiceProvider serviceProvider, IOptions<StockSettings> stockSettings)
    : IHostedService, IDisposable
{
    private readonly StockSettings _stockSettings = stockSettings.Value;
}
```

**Minimal API Route Handlers:**

```csharp
// ✅ PREFERRED: Use MapGroup for related endpoints
public static void MapStockRoutes(this WebApplication app)
{
    var stockRoutes = app.MapGroup("/stocks");
    
    stockRoutes.MapGet("/", (StockStore store) => Results.Ok(store.GetAllStocks()));
    
    stockRoutes.MapGet("/{ticker}", (StockStore store, string ticker) =>
    {
        var stock = store.GetStockByTicker(ticker);
        return stock is not null ? Results.Ok(stock) : Results.NotFound();
    });
}
```

**Anti-Patterns to Avoid:**

```csharp
// ❌ AVOID: Manual backing fields when field keyword can be used
private string _companyName;
public string CompanyName
{
    get => _companyName;
    set => _companyName = value?.Trim() ?? string.Empty;
}

// ❌ AVOID: Controllers - use Minimal API instead
[ApiController]
[Route("[controller]")]
public class StocksController : ControllerBase
{
    // Don't use controllers in this project
}

// ❌ AVOID: Constructor injection without primary constructors
public class StockStore
{
    private readonly IConfiguration _configuration;
    
    public StockStore(IConfiguration configuration)
    {
        _configuration = configuration;
    }
}
```

## Best Practices

### Error Handling

- **Validation Errors**: Use DataAnnotations on DTOs (`[Required]`, `[StringLength]`, `[Range]`)
- **Property Validation**: Throw `ArgumentException` in property setters for domain validation
- **Async Exceptions**: Swallow SSE client disconnect exceptions (TaskCanceledException)
- **Background Service Errors**: Catch and suppress exceptions in timer callbacks to prevent crashes

```csharp
// ✅ Property validation pattern
set
{
    var trimmed = value?.Trim() ?? string.Empty;
    if (string.IsNullOrWhiteSpace(trimmed))
    {
        throw new ArgumentException("Company name cannot be empty or whitespace.", nameof(CompanyName));
    }
    field = trimmed;
}

// ✅ SSE error handling
try
{
    await context.Response.Body.WriteAsync(payloadBytes);
    await client.Response.Body.FlushAsync();
}
catch (Exception)
{
    // Client disconnected - expected behavior
}
```

### Async Operations

- **Prefer async/await**: All I/O operations should be async
- **Use `Task.Delay` with cancellation tokens**: For keeping SSE connections alive
- **Background services**: Implement `IHostedService` for continuous operations
- **Scoped services in background**: Use `IServiceProvider.CreateScope()` to access scoped/transient services from singletons

```csharp
// ✅ Accessing scoped services from background service
using var scope = serviceProvider.CreateScope();
var stockStore = scope.ServiceProvider.GetRequiredService<StockStore>();
```

### State Management

- **Thread-safe collections**: Use `ConcurrentDictionary` for shared mutable state
- **Singleton services**: StockStore and SseService are singletons - must be thread-safe
- **Immutable where possible**: Stock entities use `init` on TickerSymbol to prevent modification

### Performance Considerations

- **SSE Buffering**: Disable response buffering for immediate SSE delivery
- **Concurrent access**: All data store operations use thread-safe ConcurrentDictionary methods
- **Connection tracking**: Use HttpContext.Connection.Id as unique client identifier
- **Timer intervals**: Configurable via appsettings.json to control simulation frequency

```csharp
// ✅ Disable buffering for SSE
var bufferingFeature = context.Features.Get<Microsoft.AspNetCore.Http.Features.IHttpResponseBodyFeature>();
bufferingFeature?.DisableBuffering();
```

### Security Guidelines

- **Input validation**: Always validate via DataAnnotations before use
- **Input normalization**: Trim and normalize user input in DTOs (especially ticker symbols)
- **HTTPS Redirection**: Always enabled via `app.UseHttpsRedirection()`
- **No authentication**: This is a demonstration project without auth requirements

## Testing

**Testing Framework:** No tests currently implemented

**Future Testing Recommendations:**
- Use xUnit for unit tests
- Test SSE connection handling
- Test concurrent price updates
- Mock IConfiguration for StockStore testing
- Integration tests for full API endpoints

**Test Organization:**
- Create separate `StockMarketSim.Api.Tests` project
- Name test files with `.Tests.cs` suffix (e.g., `StockStoreTests.cs`)
- Use `[Fact]` for single test scenarios, `[Theory]` for parameterized tests

## Common Patterns

### Server-Sent Events (SSE) Pattern

**When to Use:** For one-way real-time updates from server to multiple clients

**Implementation:**
```csharp
// 1. Set SSE headers
context.Response.Headers.ContentType = "text/event-stream";
context.Response.Headers.CacheControl = "no-cache";
context.Response.Headers.Connection = "keep-alive";

// 2. Disable buffering
var bufferingFeature = context.Features.Get<IHttpResponseBodyFeature>();
bufferingFeature?.DisableBuffering();

// 3. Add client to service
sseService.AddClient(context);

// 4. Keep connection alive until cancelled
try
{
    await Task.Delay(Timeout.Infinite, context.RequestAborted);
}
catch (TaskCanceledException)
{
    // Normal disconnection
}
finally
{
    sseService.RemoveClient(context);
}
```

### Background Service Pattern

**When to Use:** For continuous background operations

**Implementation:**
```csharp
public class StockPriceSimulator(IServiceProvider serviceProvider, IOptions<StockSettings> stockSettings)
    : IHostedService, IDisposable
{
    private Timer? _timer;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(interval));
        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }
    
    public void Dispose()
    {
        _timer?.Dispose();
    }
}
```

### Configuration Binding Pattern

**When to Use:** Loading structured configuration from appsettings.json

**Implementation:**
```csharp
// 1. Create strongly-typed settings class
public class StockSettings
{
    public required int UpdateIntervalSeconds { get; init; }
}

// 2. Register in Program.cs
builder.Services.Configure<StockSettings>(builder.Configuration.GetSection("StockSettings"));

// 3. Inject via IOptions<T>
public class MyService(IOptions<StockSettings> settings)
{
    private readonly StockSettings _settings = settings.Value;
}
```

### Route Grouping Pattern

**When to Use:** Organizing related endpoints with common path prefix

**Implementation:**
```csharp
public static void MapStockRoutes(this WebApplication app)
{
    var stockRoutes = app.MapGroup("/stocks");
    
    stockRoutes.MapGet("/", handler);
    stockRoutes.MapGet("/{ticker}", handler);
    stockRoutes.MapPost("/", handler);
    stockRoutes.MapPut("/{ticker}", handler);
    stockRoutes.MapDelete("/{ticker}", handler);
}
```

## Common Anti-Patterns

### Blocking Calls in Async Methods

**Why to Avoid:** Can cause deadlocks and performance issues

**Instead, Do This:**
```csharp
// ❌ AVOID
public async Task SendUpdates()
{
    var data = GetData().Result; // Blocking!
}

// ✅ CORRECT
public async Task SendUpdates()
{
    var data = await GetData();
}
```

### Storing HttpContext Long-Term

**Why to Avoid:** HttpContext should only be used for the duration of the request

**Instead, Do This:**
```csharp
// ✅ CORRECT: For SSE, this is acceptable because the request lasts as long as the connection
// The connection IS the request, and we properly clean up on disconnect
sseService.AddClient(context);
```

### Not Disposing IServiceScope

**Why to Avoid:** Memory leaks from undisposed scopes

**Instead, Do This:**
```csharp
// ✅ CORRECT
using var scope = serviceProvider.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<MyService>();
// Scope disposed automatically
```

## Dependencies

**Adding Dependencies:**
- Add via `dotnet add package PackageName` command
- Ensure version compatibility with .NET 10
- Prefer official Microsoft packages for ASP.NET Core features

**Current Dependencies:**
- `Microsoft.AspNetCore.OpenApi` (v10.0.1) - OpenAPI spec generation
- `Scalar.AspNetCore` (v2.11.6) - Interactive API documentation UI

**Updating Dependencies:**
- Test thoroughly after updating to new .NET versions
- Check for breaking changes in Scalar.AspNetCore
- Review OpenAPI spec generation after updates

## Development Workflow

**Setup:**
```bash
cd StockMarketSim.Api
dotnet restore
```

**Building:**
```bash
dotnet build
```

**Running Locally:**
```bash
dotnet run
# Access API at: https://localhost:7214 or http://localhost:5136
# View Scalar UI at: https://localhost:7214/scalar/v1 (Development only)
```

**Running with Docker:**
```bash
# From solution root
docker build -t stockmarketsim-api -f StockMarketSim.Api/Dockerfile .
docker run -d -p 8080:8080 -p 8081:8081 --name stockmarketsim stockmarketsim-api
```

**Verification Scripts:**
```bash
# Test SSE endpoint
./test-sse.sh

# Verify C# 14 features
./verify-csharp14.sh

# View OpenAPI spec
./view-openapi.sh
```

## Documentation

- **Primary Documentation**: README.md in repository root
- **Implementation Details**: `/docs` folder contains:
  - `Plan.md` - Original implementation plan
  - `CSharp14-Implementation.md` - C# 14 feature usage details
  - `OpenAPI-Guide.md` - API documentation guide
  - `README-Update-Summary.md` - Documentation changelog
- **Code Comments**: Minimal inline comments; code should be self-documenting
- **XML Documentation**: Not currently used; consider adding for public APIs
- **API Documentation**: Auto-generated via OpenAPI and displayed in Scalar UI

### Code Comment Conventions

- Use comments sparingly; prefer clear naming and structure
- Add comments for non-obvious workarounds or complex logic
- Document why, not what (code shows what)

```csharp
// ✅ Good comment - explains WHY
// Disable response buffering to ensure immediate SSE delivery
bufferingFeature?.DisableBuffering();

// ❌ Unnecessary comment - code is self-explanatory
// Get all stocks from the store
var stocks = store.GetAllStocks();
```

## CI/CD

**Current Status:** No CI/CD pipeline configured

**Recommended Setup:**
- GitHub Actions for automated builds and tests
- Docker image publishing to container registry
- Automated deployment on merge to main
- Branch protection requiring PR reviews

## Additional Context

### C# 14 Feature Usage

This project demonstrates modern C# 14 features extensively:

1. **`field` keyword**: Used throughout models and DTOs for implicit backing field access with custom logic
2. **Primary constructors**: Used for all services requiring dependency injection
3. **Collection expressions**: Used sparingly with empty collection syntax `[]`
4. **Required properties**: Combined with `required` keyword for mandatory initialization

### SSE Protocol Details

The SSE implementation follows the standard format:
- Content-Type: `text/event-stream`
- Message format: `data: {json}\n\n`
- Automatic reconnection handled by browser EventSource API
- Connection kept alive using `Task.Delay(Timeout.Infinite, cancellationToken)`

### Initial Stock Data

The application seeds 10 stocks on startup from `appsettings.json`:
- MSFT, AAPL, GOOGL, AMZN, TSLA, NVDA, META, JPM, V, JNJ
- Stock prices update randomly every 5 seconds (configurable via `StockSettings.UpdateIntervalSeconds`)

### Development Environment

- **IDE**: JetBrains Rider (evidenced by obj/ files)
- **OS**: macOS (based on development scripts using shell)
- **.NET Version**: 10.0 (preview/latest)
- **OpenAPI UI**: Scalar (modern alternative to Swagger UI)

## Resources

- [ASP.NET Core Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)
- [C# 14 What's New](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14)
- [Server-Sent Events Specification](https://html.spec.whatwg.org/multipage/server-sent-events.html)
- [Scalar API Documentation](https://github.com/scalar/scalar)
- [.NET 10 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10)

---

**Last Updated:** December 13, 2025  
**Project Version:** Stock Market Simulator API v1.0  
**Maintained By:** Project contributors

