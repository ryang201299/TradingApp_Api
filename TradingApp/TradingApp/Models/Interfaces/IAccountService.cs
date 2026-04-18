using TradingApp.Models.DTO;

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
    Task<Result<List<AccountDto>>> GetAccountsAsync();

    /// <summary>
    /// Retrieves information about an account
    /// </summary>
    /// <param name="id">Account Id</param>
    /// <returns>Information about an account</returns>
    Task<Result<AccountDto?>> GetAccountAsync(int id);
}