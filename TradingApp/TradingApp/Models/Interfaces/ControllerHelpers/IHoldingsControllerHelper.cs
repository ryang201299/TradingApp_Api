namespace TradingApp.Models.Interfaces.ControllerHelpers;
public interface IHoldingsControllerHelper
{
    /// <summary>
    /// Retrieves overall holdings value per account
    /// </summary>
    /// <returns>Overall holdings value for all accounts</returns>
    Task<Result<List<AccountHolding>>> GetOverallHoldingsForAllAccountsAsync();

    /// <summary>
    /// Retrieves overall holdings value for an account
    /// </summary>
    /// <param name="accountId">Account Id</param>
    /// <returns>Overall holdings value for account</returns>
    Task<Result<AccountHolding>> GetOverallHoldingsForAnAccountAsync(int accountId);

    /// <summary>
    /// Retrieves all individual holdings for an account
    /// </summary>
    /// <param name="accountId">Account Id</param>
    /// <returns>All individual holdings for an account</returns>
    Task<Result<AccountHoldings>> GetHoldingsForAnAccountAsync(int accountId);
}
