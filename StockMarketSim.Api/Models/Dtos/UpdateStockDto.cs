using System.ComponentModel.DataAnnotations;

namespace StockMarketSim.Api.Models.Dtos;

public class UpdateStockDto
{
    [StringLength(100)]
    public string? CompanyName
    {
        get;
        init => field = value?.Trim();
    }

    [Range(0, 10000)]
    public decimal? Price
    {
        get;
        init => field = value.HasValue ? Math.Round(Math.Max(0, value.Value), 2) : null;
    }
}



