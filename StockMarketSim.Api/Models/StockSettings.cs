namespace StockMarketSim.Api.Models;

public class StockSettings
{
    public required int UpdateIntervalSeconds
    {
        get;
        init
        {
            if (value < 1)
            {
                throw new ArgumentException("Update interval must be at least 1 second.", nameof(UpdateIntervalSeconds));
            }
            field = value;
        }
    }
}



