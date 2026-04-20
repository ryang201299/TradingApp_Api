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
    Task<Result<Account?>> GetAccountAsync(int id);

    Task<Result<int?>> GetAccountIdAsync(string accountName);
}