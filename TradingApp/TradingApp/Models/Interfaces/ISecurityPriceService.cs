using TradingApp.Models.Database;

namespace TradingApp.Models.Interfaces;

public interface ISecurityPriceService
{
    /// <summary>
    /// Retrieves latest prices for all securities
    /// </summary>
    /// <returns>Latest price for all securities</returns>
    Task<Result<List<SecurityPrice>>> GetSecurityPricesAsync();

    /// <summary>
    /// Retrieves latest price for one security
    /// </summary>
    /// <param name="securityId">Id of the security</param>
    /// <returns>Latest price for this security</returns>
    Task<Result<SecurityPrice>> GetSecurityPriceAsync(int securityId);
}