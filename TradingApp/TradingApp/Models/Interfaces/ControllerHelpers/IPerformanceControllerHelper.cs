namespace TradingApp.Models.Interfaces.ControllerHelpers;
public interface IPerformanceControllerHelper
{
    Task<List<AccountUnrealisedReturns>> GetUnrealisedReturns();
}