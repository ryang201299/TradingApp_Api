using TradingApp.Models.Database;

namespace TradingApp.Models;
public class SecurityHolding
{
    public required Security Security { get; set; }

    public required decimal Holding { get; set; }
}