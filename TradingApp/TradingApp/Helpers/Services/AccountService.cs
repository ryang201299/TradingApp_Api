using Microsoft.EntityFrameworkCore;
using TradingApp.Data;
using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.Interfaces;

namespace TradingApp.Helpers.Services;

/// <inheritdoc cref="IAccountService" />
public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly TradingAppContext _context;

    public AccountService(ILogger<AccountService> logger, TradingAppContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Result<List<Account>>> GetAccountsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving accounts...");

            List<Account> accounts = await _context.Accounts.ToListAsync();

            _logger.LogInformation("`{CountOfAccounts}` accounts returned.", accounts.Count);

            return Result<List<Account>>.Success(accounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving accounts.");

            return Result<List<Account>>.Failure(ex.Message);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Account?>> GetAccountAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving account `{AccountId}`...", id);

            Account? account = await _context.Accounts
                .Where(x => x.AccountId == id)
                .FirstOrDefaultAsync();

            if (account == null)
            {
                _logger.LogInformation("Account `{AccountId}` not found.", id);
            }
            else
            {
                _logger.LogInformation("Retrieved account id `{AccountId}` with name `{AccountName}`.", account.AccountId, account.Name);
            }

            return Result<Account?>.Success(account);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving account `{AccountId}`.", id);

            return Result<Account?>.Failure(ex.Message);
        }
    }

    public async Task<Result<int?>> GetAccountIdAsync(string name)
    {
        try
        {
            _logger.LogInformation("Retrieving account id given account name `{AccountName}`...", name);

            int? accountId = await _context.Accounts
                .Where(x => x.Name == name)
                .Select(x => x.AccountId).FirstOrDefaultAsync();

            if (accountId == null)
            {
                _logger.LogInformation("Account `{AccountName}` not found.", name);
            }
            else
            {
                _logger.LogInformation("Retrieved account id `{AccountId}` given name `{AccountName}`", accountId, name);
            }

            return Result<int?>.Success(accountId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving account `{AccountName}`.", name);

            return Result<int?>.Failure(ex.Message);
        }
    }
}