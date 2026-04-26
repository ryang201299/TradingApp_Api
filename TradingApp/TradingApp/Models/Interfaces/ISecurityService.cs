using TradingApp.Models.Database;

namespace TradingApp.Models.Interfaces;

public interface ISecurityService
{
    Task<Result<List<Security>>> GetSecuritiesAsync();

    Task<Result<int>> GetSecurityIdAsync(string securityName);

    Task<Result<Security>> GetSecurityAsync(int id);
}