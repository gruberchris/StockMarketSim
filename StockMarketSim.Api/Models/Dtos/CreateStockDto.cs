using System.ComponentModel.DataAnnotations;

namespace StockMarketSim.Api.Models.Dtos;

public class CreateStockDto
{
    [Required]
    [StringLength(10)]
    public required string TickerSymbol
    {
        get => field;
        init => field = value?.Trim().ToUpperInvariant() ?? string.Empty;
    }

    [Required]
    [StringLength(100)]
    public required string CompanyName
    {
        get => field;
        init => field = value?.Trim() ?? string.Empty;
    }

    [Range(0, 10000)]
    public decimal Price
    {
        get => field;
        init => field = Math.Round(Math.Max(0, value), 2);
    }
}



