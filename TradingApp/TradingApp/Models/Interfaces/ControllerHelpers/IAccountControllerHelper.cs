using TradingApp.Models.Database;

namespace TradingApp.Models.Interfaces;

/// <summary>
/// Helper class adding additional buisness logic error 
/// handling and logging to the account service class
/// </summary>
public interface IAccountControllerHelper
{
    /// <summary>
    /// Retrieves all accounts
    /// </summary>
    /// <returns>List of accounts and their information</returns>
    Task<Result<List<Account>>> GetAccountsAsync();

    /// <summary>
    /// Retrieves an account if it exists
    /// </summary>
    /// <param name="id">AccountId</param>
    /// <returns>Account details</returns>
    Task<Result<Account?>> GetAccountAsync(int id);

    Task<Result<int?>> GetAccountIdAsync(string name);
}