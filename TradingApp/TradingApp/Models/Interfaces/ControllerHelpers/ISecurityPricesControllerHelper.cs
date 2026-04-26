using TradingApp.Models.Database;

namespace TradingApp.Models.Interfaces.ControllerHelpers;

public interface ISecurityPricesControllerHelper
{
    /// <summary>
    /// Retrieves latest prices for all securities
    /// </summary>
    /// <returns>Latest prices for all securities</returns>
    Task<Result<List<SecurityPrice>>> GetSecurityPricesAsync();
}
