using TradingApp.Models.Database;

namespace TradingApp.Models;

public class AccountHolding
{
    public required Account Account { get; set; }
    public required decimal Holding { get; set; }
}
