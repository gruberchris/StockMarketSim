namespace StockMarketSim.Api.Models;

public class Stock
{
    public required string TickerSymbol { get; init; }

    public required string CompanyName
    {
        get;
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

    public decimal Price
    {
        get;
        set => field = Math.Round(Math.Max(0, value), 2);
    }
}



