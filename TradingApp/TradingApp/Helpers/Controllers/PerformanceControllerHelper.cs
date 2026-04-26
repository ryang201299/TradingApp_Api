using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.Interfaces;
using TradingApp.Models.Interfaces.ControllerHelpers;

namespace TradingApp.Helpers.Controllers;
public class PerformanceControllerHelper : IPerformanceControllerHelper
{
    private readonly ILogger<PerformanceControllerHelper> _logger;
    private readonly ITransactionService _transactionService;
    private readonly ISecurityPriceService _securityPriceService;

    public PerformanceControllerHelper(ILogger<PerformanceControllerHelper> logger, ITransactionService transactionService, ISecurityPriceService securityPriceService)
    {
        _logger = logger;
        _transactionService = transactionService;
        _securityPriceService = securityPriceService;
    }

    /// <summary>
    /// Calculates the total share cost of all holdings for all accounts
    /// </summary>
    /// <param name="transactions">All transactions for all accounts</param>
    /// <returns>Total share cost of all holdings for all accounts</returns>
    private Result<List<HoldingsShareCost>> CalculateShareCost(List<Transaction> transactions)
    {
        _logger.LogInformation("Calulating share cost for all accounts.");

        try
        {
            // get total value of all held quantities at original cost
            var shareCost = transactions
                .GroupBy(x => new
                {
                    x.Account,
                    x.Security,
                    x.SecurityPrice
                })
                .Select(g => new
                {
                    Account = g.Key.Account,
                    Security = g.Key.Security,
                    TotalHeld = g.Sum(e => e.TransactionType.TransactionTypeId == 1 ? e.Quantity : -e.Quantity),
                    Price = g.Key.SecurityPrice
                }).ToList();

            // total cost of shares per account
            var shareCostPerAccount = shareCost
                .GroupBy(x => x.Account)
                .Select(g => new HoldingsShareCost()
                {
                    Account = g.Key,
                    ShareCost = g.Sum(e => e.TotalHeld * e.Price)
                }).ToList();

            return Result<List<HoldingsShareCost>>.Success(shareCostPerAccount);
        }
        catch (Exception ex)
        {
            return Result<List<HoldingsShareCost>>.Failure(ex.Message);
        }
    }
    
    // Can be refactored to remove unnecessary group by on account
    /// <summary>
    /// Calculates total share cost for an account
    /// </summary>
    /// <param name="transactions">All transactions for an account</param>
    /// <returns>Total share cost for an accoutn</returns>
    private Result<HoldingsShareCost> CalculateShareCostForAnAccount(List<Transaction> transactions)
    {
        _logger.LogInformation("Calulating share cost for account `{AccountId}`.", transactions.Select(x => x.Account.AccountId).First());

        try
        {
            // get total value of all held quantities at original cost
            var shareCost = transactions
                .GroupBy(x => new
                {
                    x.Account,
                    x.Security,
                    x.SecurityPrice
                })
                .Select(g => new
                {
                    Account = g.Key.Account,
                    Security = g.Key.Security,
                    TotalHeld = g.Sum(e => e.TransactionType.TransactionTypeId == 1 ? e.Quantity : -e.Quantity),
                    Price = g.Key.SecurityPrice
                }).ToList();

            // total cost of shares per account
            var shareCostPerAccount = shareCost
                .GroupBy(x => x.Account)
                .Select(g => new HoldingsShareCost()
                {
                    Account = g.Key,
                    ShareCost = g.Sum(e => e.TotalHeld * e.Price)
                }).First();

            return Result<HoldingsShareCost>.Success(shareCostPerAccount);
        }
        catch (Exception ex)
        {
            return Result<HoldingsShareCost>.Failure(ex.Message);
        }

    }

    /// <summary>
    /// Calculates total share worth for all holdings across all accounts
    /// </summary>
    /// <param name="transactions">All transactions for all accounts</param>
    /// <returns>Total share worth for all holdings across all accounts</returns>
    private async Task<Result<List<HoldingsShareWorth>>> CalculateShareWorth(List<Transaction> transactions)
    {
        try
        {
            // get total value of all held qunaities at latest price
            Result<List<SecurityPrice>> securityPricesResult = await _securityPriceService.GetSecurityPricesAsync();

            if (!securityPricesResult.IsSuccess)
            {
                return Result<List<HoldingsShareWorth>>.Failure(securityPricesResult.Error!);
            }

            var holdingsPerAccount = transactions
                .GroupBy(x => new
                {
                    x.Account,
                    x.Security
                })
                .Select(g => new
                {
                    Account = g.Key.Account,
                    Security = g.Key.Security,
                    TotalHeld = g.Sum(e => e.TransactionType.TransactionTypeId == 1 ? e.Quantity : -e.Quantity),
                }).ToList();

            var shareWorth =
                from
                    accountHolding in holdingsPerAccount

                join
                    securityPrice in securityPricesResult.Value!
                        on accountHolding.Security.SecurityId equals securityPrice.SecurityId

                select new
                {
                    Account = accountHolding.Account,
                    Security = accountHolding.Security,
                    Held = accountHolding.TotalHeld,
                    Price = securityPrice.Price
                };

            var shareWorthByAccount = shareWorth
                .GroupBy(x => x.Account)
                .Select(g => new HoldingsShareWorth()
                {
                    Account = g.Key,
                    ShareWorth = g.Sum(e => e.Held * e.Price)
                }).ToList();

            return Result<List<HoldingsShareWorth>>.Success(shareWorthByAccount);
        }
        catch (Exception ex)
        {
            return Result<List<HoldingsShareWorth>>.Failure(ex.Message);
        }
    }

    // Can be refactored to remove unnecessary group by on account
    /// <summary>
    /// Calculates total share worth for an account
    /// </summary>
    /// <param name="transactions">All transactions for an account</param>
    /// <returns>Total share worth for an account</returns>
    private async Task<Result<HoldingsShareWorth>> CalculateShareWorthForAnAccount(List<Transaction> transactions)
    {
        try
        {
            // get total value of all held qunaities at latest price
            Result<List<SecurityPrice>> securityPricesResult = await _securityPriceService.GetSecurityPricesAsync();

            if (!securityPricesResult.IsSuccess)
            {
                return Result<HoldingsShareWorth>.Failure(securityPricesResult.Error!);
            }

            var holdingsPerAccount = transactions
                .GroupBy(x => new
                {
                    x.Account,
                    x.Security
                })
                .Select(g => new
                {
                    Account = g.Key.Account,
                    Security = g.Key.Security,
                    TotalHeld = g.Sum(e => e.TransactionType.TransactionTypeId == 1 ? e.Quantity : -e.Quantity),
                }).ToList();

            var shareWorth =
                from
                    accountHolding in holdingsPerAccount

                join
                    securityPrice in securityPricesResult.Value!
                        on accountHolding.Security.SecurityId equals securityPrice.SecurityId

                select new
                {
                    Account = accountHolding.Account,
                    Security = accountHolding.Security,
                    Held = accountHolding.TotalHeld,
                    Price = securityPrice.Price
                };

            var shareWorthByAccount = shareWorth
                .GroupBy(x => x.Account)
                .Select(g => new HoldingsShareWorth()
                {
                    Account = g.Key,
                    ShareWorth = g.Sum(e => e.Held * e.Price)
                }).First();

            return Result<HoldingsShareWorth>.Success(shareWorthByAccount);
        }
        catch (Exception ex)
        {
            return Result<HoldingsShareWorth>.Failure(ex.Message);
        }
    }

    /// <inheritdoc />
    public async Task<Result<List<AccountUnrealisedReturns>>> GetUnrealisedReturnsAsync()
    {
        _logger.LogInformation("Retrieving unrealised returns for all accounts.");

        // Retrieving required data
        Result<List<Transaction>> transactionsResult = await _transactionService.GetTransactionsAsync();

        if (!transactionsResult.IsSuccess)
        {
            return Result<List<AccountUnrealisedReturns>>.Failure(transactionsResult.Error!);
        }

        Result<List<HoldingsShareCost>> shareCostResult = CalculateShareCost(transactionsResult.Value!);

        if (!shareCostResult.IsSuccess)
        { 
            return Result<List<AccountUnrealisedReturns>>.Failure(shareCostResult.Error!);
        }

        Result<List<HoldingsShareWorth>> shareWorthResult = await CalculateShareWorth(transactionsResult.Value!);

        if (!shareWorthResult.IsSuccess)
        {
            return Result<List<AccountUnrealisedReturns>>.Failure(shareWorthResult.Error!);
        }

        // unrealised returns = ((newPrice - oldPrice) / oldPrice) * 100
        List<AccountUnrealisedReturns> unrealisedReturns = shareWorthResult.Value!
            .Select(x => new AccountUnrealisedReturns()
            {
                Account = x.Account,
                UnrealisedReturns =
                Math.Round(
                (
                    (x.ShareWorth - shareCostResult.Value!.FirstOrDefault(e => e.Account == x.Account)!.ShareCost)
                    / shareCostResult.Value!.FirstOrDefault(e => e.Account == x.Account)!.ShareCost
                ) * 100
                , 2)
            }).ToList();

        return Result<List<AccountUnrealisedReturns>>.Success(unrealisedReturns);
    }

    /// <inheritdoc />
    public async Task<Result<AccountUnrealisedReturns>> GetUnrealisedReturnsForAccountAsync(int id)
    {
        _logger.LogInformation("Retrieving unrealised returns for accounts `{AccountId}`.", id);

        // Retrieving required data
        Result<List<Transaction>> transactionsResult = await _transactionService.GetTransactionsAsync();

        if (!transactionsResult.IsSuccess)
        {
            return Result<AccountUnrealisedReturns>.Failure(transactionsResult.Error!);
        }

        List<Transaction> accountTransactions = transactionsResult.Value!.Where(x => x.Account.AccountId == id).ToList();

        Result<HoldingsShareCost> shareCostResult = CalculateShareCostForAnAccount(accountTransactions);

        if (!shareCostResult.IsSuccess)
        {
            return Result<AccountUnrealisedReturns>.Failure(shareCostResult.Error!);
        }

        Result<HoldingsShareWorth> shareWorthResult = await CalculateShareWorthForAnAccount(accountTransactions);

        if (!shareWorthResult.IsSuccess)
        {
            return Result<AccountUnrealisedReturns>.Failure(shareWorthResult.Error!);
        }

        AccountUnrealisedReturns unrealisedReturns = new()
        {
            Account = shareWorthResult.Value!.Account,
            UnrealisedReturns = ((shareWorthResult.Value.ShareWorth - shareCostResult.Value!.ShareCost) / shareCostResult.Value!.ShareCost) * 100
        };

        return Result<AccountUnrealisedReturns>.Success(unrealisedReturns);
    }
}
