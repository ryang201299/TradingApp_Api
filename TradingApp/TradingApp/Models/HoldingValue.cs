using TradingApp.Models.Database;

namespace TradingApp.Models;
public class HoldingValue
{
    public required Account Account { get; set; }

    public required Security Security { get; set; }

    public required decimal Value { get; set; }
}
