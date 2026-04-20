using Microsoft.EntityFrameworkCore;
using TradingApp.Data;
using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.Interfaces;

namespace TradingApp.Helpers.Services;

public class TransactionService : ITransactionService
{
    private readonly ILogger<TransactionService> _logger;
    private readonly TradingAppContext _context;

    public TransactionService(ILogger<TransactionService> logger, TradingAppContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Transaction>>> GetTransactionsAsync(int accountId)
    {
        try
        {
            _logger.LogInformation("Retrieving transactions for account `{AccountId}`.", accountId);
            List<Transaction> transactions = await _context.Transactions
                .Where(x => x.Account.AccountId == accountId)
                .Include(x => x.Account)
                .Include(x => x.TransactionType)
                .Include(x => x.Security)
                .ToListAsync();

            _logger.LogInformation("Retrieve `{TransactionCount}` transactions.", transactions.Count);

            return Result<List<Transaction>>.Success(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred retrieving transactions for account `{AccountId}`.", accountId);
            return Result<List<Transaction>>.Failure(ex.Message);
        }
    }

    public async Task<Result> BuySecurityAsync(Transaction transaction)
    {
        try
        {
            _logger.LogInformation("Adding new transaction for " +
                "account `{AccountId}`, " +
                "security `{SecurityId}`, and " +
                "transaction type `{TransactionType}`.",
                transaction.Account.AccountId,
                transaction.Security.SecurityId,
                transaction.TransactionType.TransactionTypeId);

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating tranasction for " +
                "account `{AccountId}`, " +
                "security `{SecurityId}`, and " +
                "transaction type `{TransactionType}`.",
                transaction.Account.AccountId,
                transaction.Security.SecurityId,
                transaction.TransactionType.TransactionTypeId);

            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result> SellSecurityAsync(Transaction transaction)
    {
        try
        {
            _logger.LogInformation("Adding new transaction for " +
                "account `{AccountId}`, " +
                "security `{SecurityId}`, and " +
                "transaction type `{TransactionType}`.",
                transaction.Account.AccountId,
                transaction.Security.SecurityId,
                transaction.TransactionType.TransactionTypeId);

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred creating tranasction for " +
                "account `{AccountId}`, " +
                "security `{SecurityId}`, and " +
                "transaction type `{TransactionType}`.",
                transaction.Account.AccountId,
                transaction.Security.SecurityId,
                transaction.TransactionType.TransactionTypeId);

            return Result.Failure(ex.Message);
        }
    }
}