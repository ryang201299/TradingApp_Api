using TradingApp.Models.Database;

namespace TradingApp.Models.Interfaces;

public interface ISecurityPriceService
{
    Task<Result<List<SecurityPrice>>> GetSecurityPricesAsync();

    Task<Result<SecurityPrice?>> GetSecurityPriceAsync(int securityId);
}