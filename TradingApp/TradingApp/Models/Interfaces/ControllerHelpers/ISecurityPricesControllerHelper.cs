using TradingApp.Models.Database;

namespace TradingApp.Models.Interfaces.ControllerHelpers;

public interface ISecurityPricesControllerHelper
{
    Task<Result<List<SecurityPrice>>> GetSecurityPricesAsync();
}
