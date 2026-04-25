using TradingApp.Models.Database;

namespace TradingApp.Models;
public class AccountUnrealisedReturns
{
    public required Account Account { get; set; }

    public required decimal UnrealisedReturns { get; set; }
}
