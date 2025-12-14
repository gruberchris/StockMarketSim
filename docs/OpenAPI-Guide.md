# OpenAPI Documentation - Quick Reference

## 🎯 How to View the API Documentation

Your Stock Market Simulator API now has **two ways** to view OpenAPI documentation:

### ✨ Option 1: Scalar UI (RECOMMENDED)
**Beautiful, interactive API documentation**

**URL:** `https://localhost:7214/scalar/v1`

**Features:**
- 🎨 Modern, clean interface
- 🧪 Test API endpoints directly in the browser
- 📖 Auto-generated from your code
- 🔍 Search and filter endpoints
- 📝 Request/response examples

**How to access:**
```bash
# Open in your default browser
open https://localhost:7214/scalar/v1

# Or use the helper script
./view-openapi.sh
```

### 📄 Option 2: OpenAPI JSON Specification
**Raw OpenAPI 3.0 specification document**

**URL:** `https://localhost:7214/openapi/v1.json`

**Use this if you want to:**
- Import into Postman/Insomnia
- Generate client SDKs
- Integrate with other tools
- View the raw specification

**How to access:**
```bash
# View in browser
open https://localhost:7214/openapi/v1.json

# View formatted in terminal (requires jq)
curl -k https://localhost:7214/openapi/v1.json | jq

# Download to file
curl -k https://localhost:7214/openapi/v1.json > openapi-spec.json
```

## 🚀 Quick Start

1. **Make sure your app is running:**
   ```bash
   cd StockMarketSim.Api
   dotnet run
   ```

2. **Access the Scalar UI:**
   - Open browser to: `https://localhost:7214/scalar/v1`
   - OR run: `./view-openapi.sh` and choose option 1

3. **You'll see all your endpoints:**
   - `GET /stocks` - Get all stocks
   - `GET /stocks/{ticker}` - Get specific stock
   - `POST /stocks` - Create new stock
   - `PUT /stocks/{ticker}` - Update stock
   - `DELETE /stocks/{ticker}` - Delete stock
   - `GET /stocks/live` - SSE endpoint for real-time updates

## 📋 API Endpoints Summary

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/stocks` | Get all stocks |
| GET | `/stocks/{ticker}` | Get stock by ticker symbol |
| POST | `/stocks` | Create a new stock |
| PUT | `/stocks/{ticker}` | Update existing stock |
| DELETE | `/stocks/{ticker}` | Delete stock |
| GET | `/stocks/live` | SSE stream for real-time price updates |
| GET | `/openapi/v1.json` | OpenAPI specification |
| GET | `/scalar/v1` | Interactive API documentation |

## 🔧 Configuration

OpenAPI is only available in **Development** mode (which is the default when running locally).

Configured in `Program.cs`:
```csharp
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();        // Serves /openapi/v1.json
    app.MapScalarApiReference();  // Serves /scalar/v1
}
```

## 📦 Packages Used

- **Microsoft.AspNetCore.OpenApi** - OpenAPI specification generation
- **Scalar.AspNetCore** - Beautiful UI for API documentation

## 💡 Tips

1. **Testing Endpoints**: Use the Scalar UI to test endpoints directly without needing Postman
2. **API Design**: The OpenAPI spec is auto-generated from your minimal API code
3. **Client Generation**: Export the JSON spec to generate client libraries for other languages
4. **Documentation**: All your DTOs, validation rules, and routes are automatically documented

## 🛠️ Helper Scripts

### `view-openapi.sh`
Interactive script to access OpenAPI documentation:
```bash
./view-openapi.sh
```

Choose from:
- Option 1: Open Scalar UI
- Option 2: Open JSON spec
- Option 3: View formatted spec in terminal

### `test-sse.sh`
Test the SSE endpoint for real-time stock updates:
```bash
./test-sse.sh
```

## ❓ Troubleshooting

**Problem:** Can't access OpenAPI routes
- ✅ Make sure app is running in Development mode
- ✅ Check you're using HTTPS: `https://localhost:7214`
- ✅ Accept the self-signed certificate in your browser

**Problem:** Routes not showing up in Scalar
- ✅ Rebuild the app: `dotnet build`
- ✅ Restart the app
- ✅ Clear browser cache

**Problem:** SSL certificate error
- ✅ Add `-k` flag to curl commands to skip verification
- ✅ Or trust the development certificate: `dotnet dev-certs https --trust`

