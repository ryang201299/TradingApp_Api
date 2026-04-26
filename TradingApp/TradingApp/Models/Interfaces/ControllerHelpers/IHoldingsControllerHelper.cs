namespace TradingApp.Models.Interfaces.ControllerHelpers;
public interface IHoldingsControllerHelper
{
    /// <summary>
    /// Retrieves overall holdings value per account
    /// </summary>
    /// <returns>Overall holdings value for all accounts</returns>
    Task<Result<List<HoldingsPerAccount>>> GetHoldingsAsync();
}
