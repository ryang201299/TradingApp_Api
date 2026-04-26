namespace TradingApp.Models.Interfaces.ControllerHelpers;
public interface IPerformanceControllerHelper
{
    /// <summary>
    /// Retrieves unrealised returns for all accounts
    /// </summary>
    /// <returns>Unrealised returns for all accounts</returns>
    Task<Result<List<AccountUnrealisedReturns>>> GetUnrealisedReturns();
}