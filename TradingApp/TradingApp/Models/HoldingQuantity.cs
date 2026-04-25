using TradingApp.Models.Database;

namespace TradingApp.Models;
public class HoldingQuantity
{
    public required Account Account { get; set; }

    public required Security Security { get; set; }

    public required int Quantity { get; set; }
}