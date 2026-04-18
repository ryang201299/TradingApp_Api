using TradingApp.Models.Interfaces;
using TradingApp.Models.DTO;
using TradingApp.Models;

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
    public async Task<Result<List<AccountDto>>> GetAccountsAsync()
    {
        return await _accountService.GetAccountsAsync();
    }

    /// <inheritdoc />
    public async Task<Result<AccountDto?>> GetAccountAsync(int id)
    {
        return await _accountService.GetAccountAsync(id);
    }
}