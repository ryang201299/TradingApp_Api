using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.Interfaces;

namespace TradingApp.Helpers.Controllers;

/// <inheritdoc cref="IAccountControllerHelper"/>
public class AccountControllerHelper : IAccountControllerHelper
{
    private readonly IAccountService _accountService;

    public AccountControllerHelper(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <inheritdoc />
    public async Task<Result<List<Account>>> GetAccountsAsync()
    {
        return await _accountService.GetAccountsAsync();
    }

    /// <inheritdoc />
    public async Task<Result<Account>> GetAccountAsync(int id)
    {
        return await _accountService.GetAccountAsync(id);
    }

    /// <inheritdoc />
    public async Task<Result<int>> GetAccountIdAsync(string name)
    {
        return await _accountService.GetAccountIdAsync(name);
    }
}