using Microsoft.EntityFrameworkCore;
using TradingApp.Data;
using TradingApp.Models.DTO;
using TradingApp.Models.Interfaces;
using TradingApp.Models;

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
    public async Task<Result<List<AccountDto>>> GetAccountsAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving accounts...");

            List<AccountDto> accounts = await _context.Accounts
                .Select(x => new AccountDto
                {
                    AccountId = x.AccountId,
                    Name = x.Name
                })
                .ToListAsync();

            _logger.LogInformation("`{CountOfAccounts}` accounts returned.", accounts.Count);

            return Result<List<AccountDto>>.Success(accounts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving accounts.");

            return Result<List<AccountDto>>.Failure(ex.Message);
        }
    }

    /// <inheritdoc />
    public async Task<Result<AccountDto?>> GetAccountAsync(int id)
    {
        try
        {
            _logger.LogInformation("Retrieving account `{AccountId}`...", id);

            AccountDto? account = await _context.Accounts
                .Where(x => x.AccountId == id)
                .Select(x => new AccountDto()
                {
                    AccountId = x.AccountId,
                    Name = x.Name
                }).FirstOrDefaultAsync();

            if (account == null)
            {
                _logger.LogInformation("Account `{AccountId}` not found.", id);
            }
            else
            {
                _logger.LogInformation("Retrieved account id `{AccountId}` with name `{AccountName}`.", account.AccountId, account.Name);
            }

            return Result<AccountDto?>.Success(account);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving account `{AccountId}`.", id);
            
            return Result<AccountDto?>.Failure(ex.Message);
        }
    }
}