using TradingApp.Models.Database;

namespace TradingApp.Models;
public class AccountHoldings
{
    public required Account Account { get; set; }

    public required List<SecurityHolding> Holdings { get; set; }
}
