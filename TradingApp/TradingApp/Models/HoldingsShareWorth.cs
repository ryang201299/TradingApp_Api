using TradingApp.Models.Database;

namespace TradingApp.Models;
public class HoldingsShareWorth
{
    public required Account Account { get; set; }

    public required decimal ShareWorth { get; set; }
}
