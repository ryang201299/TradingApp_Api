using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.Interfaces;

namespace TradingApp.Helpers.Controllers;

/// <inheritdoc cref="IAccountControllerHelper"/>
public class AccountControllerHelper : IAccountControllerHelper
{
    private readonly ILogger<AccountControllerHelper> _logger;
    private readonly IAccountService _accountService;

    public AccountControllerHelper(ILogger<AccountControllerHelper> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    /// <inheritdoc />
    public async Task<Result<List<Account>>> GetAccountsAsync()
    {
        return await _accountService.GetAccountsAsync();
    }

    /// <inheritdoc />
    public async Task<Result<Account?>> GetAccountAsync(int id)
    {
        return await _accountService.GetAccountAsync(id);
    }

    public async Task<Result<int?>> GetAccountIdAsync(string name)
    {
        return await _accountService.GetAccountIdAsync(name);
    }
}