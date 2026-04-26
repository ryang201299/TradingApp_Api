using TradingApp.Models.Database;

namespace TradingApp.Models.Interfaces;

/// <summary>
/// Handles low level context query execution for Account related queries
/// </summary>
public interface IAccountService
{
    /// <summary>
    /// Retrieves all accounts
    /// </summary>
    /// <returns>List of accounts and their information</returns>
    Task<Result<List<Account>>> GetAccountsAsync();

    /// <summary>
    /// Retrieves information about an account
    /// </summary>
    /// <param name="id">Account Id</param>
    /// <returns>Information about an account</returns>
    Task<Result<Account>> GetAccountAsync(int id);

    /// <summary>
    /// Reetrieves an accounts id given it's name
    /// </summary>
    /// <param name="accountName">Name of an account</param>
    /// <returns>Account Id</returns>
    Task<Result<int>> GetAccountIdAsync(string accountName);

    /// <summary>
    /// Adds cash to an account
    /// </summary>
    /// <param name="account">Account to add cash to</param>
    /// <param name="cash">Amount of cash to add</param>
    Task<Result> AddCashAsync(Account account, decimal cash);

    /// <summary>
    /// Widthdraws cash from an account
    /// </summary>
    /// <param name="account">Account to widthdraw cash from</param>
    /// <param name="cash">Amount of cash to widthdraw</param>
    Task<Result> WidthdrawCashAsync(Account account, decimal cash);
}