using TradingApp.Models.Database;

namespace TradingApp.Models.Interfaces.ControllerHelpers;

public interface ISecurityControllerHelper
{
    Task<Result<List<Security>>> GetSecuritiesAsync();

    Task<Result<int?>> GetSecurityIdAsync(string name);
}