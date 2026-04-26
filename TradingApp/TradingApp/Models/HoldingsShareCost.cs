using TradingApp.Models.Database;

namespace TradingApp.Models;
public class HoldingsShareCost
{
    public required Account Account { get; set; }
    
    public required decimal ShareCost { get; set; }
}
