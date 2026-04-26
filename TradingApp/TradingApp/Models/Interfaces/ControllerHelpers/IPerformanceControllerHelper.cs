namespace TradingApp.Models.Interfaces.ControllerHelpers;
public interface IPerformanceControllerHelper
{
    /// <summary>
    /// Retrieves unrealised returns for all accounts
    /// </summary>
    /// <returns>Unrealised returns for all accounts</returns>
    Task<Result<List<AccountUnrealisedReturns>>> GetUnrealisedReturnsAsync();

    /// <summary>
    /// Calculates unrealised returns for an account
    /// </summary>
    /// <param name="id">Account Id</param>
    /// <returns>Unrealised returns</returns>
    Task<Result<AccountUnrealisedReturns>> GetUnrealisedReturnsForAccountAsync(int id);
}