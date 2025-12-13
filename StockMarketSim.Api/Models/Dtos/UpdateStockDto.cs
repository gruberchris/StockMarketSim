using System.ComponentModel.DataAnnotations;

namespace StockMarketSim.Api.Models.Dtos;

public class UpdateStockDto
{
    [StringLength(100)]
    public string? CompanyName
    {
        get => field;
        init => field = value?.Trim();
    }

    [Range(0, 10000)]
    public decimal? Price
    {
        get => field;
        init => field = value.HasValue ? Math.Round(Math.Max(0, value.Value), 2) : null;
    }
}



