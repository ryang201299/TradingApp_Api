using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.Interfaces;
using TradingApp.Models.Interfaces.ControllerHelpers;

namespace TradingApp.Helpers.Controllers;

public class HoldingsControllerHelper : IHoldingsControllerHelper
{
    private readonly ILogger<HoldingsControllerHelper> _logger;
    private readonly ITransactionService _transactionService;
    private readonly ISecurityPriceService _securityPriceService;
    private readonly IAccountService _accountService;

    public HoldingsControllerHelper(
        ILogger<HoldingsControllerHelper> logger, 
        ITransactionService transactionService, 
        ISecurityPriceService securityPriceService, 
        IAccountService accountService)
    {
        _logger = logger;
        _transactionService = transactionService;
        _securityPriceService = securityPriceService;
        _accountService = accountService;
    }

    /// <summary>
    /// Helper method which retrieves total holdings per account per security for all accounts and securities
    /// </summary>
    /// <returns>Overall holdings for all accounts</returns>
    private async Task<Result<List<HoldingQuantity>>> GetHoldingQuantities()
    {
        _logger.LogInformation("Retrieving overall holdings for all accounts.");

        // Retrieving required data
        Result<List<Transaction>> transactionsResult = await _transactionService.GetTransactionsAsync();

        if (!transactionsResult.IsSuccess)
        {
            return Result<List<HoldingQuantity>>.Failure(transactionsResult.Error!);
        }

        List<Transaction> transactions = transactionsResult.Value!;

        var grouped = transactions
            .GroupBy(x => new
            {
                x.Account,
                x.Security
            })
            .Select(g => new
            {
                Account = g.Key.Account,
                Security = g.Key.Security,
                // Increment buys, decrement sells for total current held quantity
                TotalQuantity = g.Sum(e => e.TransactionType.TransactionTypeId == 1 ? e.Quantity : -e.Quantity)
            }).ToList();

        List<HoldingQuantity> holdingQuantities = grouped.Select(x => new HoldingQuantity()
        {
            Account = x.Account,
            Security = x.Security,
            Quantity = x.TotalQuantity
        }).ToList();

        return Result<List<HoldingQuantity>>.Success(holdingQuantities);
    }

    /// <summary>
    /// Helper method which retrieves the current value of holdings for all accounts
    /// </summary>
    /// <param name="holdingQuantities">Overall holdings for all accounts</param>
    /// <returns>Overall holding value for all accounts</returns>
    private async Task<Result<List<HoldingValue>>> GetHoldingValue(List<HoldingQuantity> holdingQuantities)
    {
        _logger.LogInformation("Retrieving holding value for all accounts.");

        // Retrieve required data
        Result<List<SecurityPrice>> securityPricesResult = await _securityPriceService.GetSecurityPricesAsync();

        if (!securityPricesResult.IsSuccess)
        {
            return Result<List<HoldingValue>>.Failure(securityPricesResult.Error!);
        }

        List<SecurityPrice> securityPrices = securityPricesResult.Value!;

        // Join holdings on latest security prices
        var joinedOnSecPrice =
            from
                groupthing in holdingQuantities

            join
                securityPrice in securityPrices
                    on groupthing.Security.SecurityId equals securityPrice.SecurityId

            select new
            {
                Account = groupthing.Account,
                Security = groupthing.Security,
                TotalHeld = groupthing.Quantity * securityPrice.Price
            };

        // Filter out none held securities
        var accountHoldingsNotZero = joinedOnSecPrice.Where(x => x.TotalHeld != 0).ToList();

        List<HoldingValue> holdingValue = accountHoldingsNotZero.Select(x => new HoldingValue()
        {
            Account = x.Account,
            Security = x.Security,
            Value = x.TotalHeld
        }).ToList();

        return Result<List<HoldingValue>>.Success(holdingValue);
    }

    /// <inheritdoc />
    public async Task<Result<List<HoldingsPerAccount>>> GetHoldingsAsync()
    {
        _logger.LogInformation("Retrieving holdings for all accounts.");

        // Retrieve overall holdings for all accounts
        Result<List<HoldingQuantity>> holdingQuantitiesResult = await GetHoldingQuantities();

        if (!holdingQuantitiesResult.IsSuccess)
        {
            return Result<List<HoldingsPerAccount>>.Failure(holdingQuantitiesResult.Error!);
        }
        
        // Retrieve value of overall holdings
        Result<List<HoldingValue>> holdingsValueResult = await GetHoldingValue(holdingQuantitiesResult.Value!);

        if (!holdingsValueResult.IsSuccess)
        {
            return Result<List<HoldingsPerAccount>>.Failure(holdingsValueResult.Error!);
        }

        // get accounts cash
        Result<List<Account>> accountsResult = await _accountService.GetAccountsAsync();

        if (!accountsResult.IsSuccess)
        { 
            return Result<List<HoldingsPerAccount>>.Failure(accountsResult.Error!);
        }

        // group by account and sum of holdings
        var accountHoldings = holdingsValueResult.Value!.GroupBy(x => x.Account).Select(g => new HoldingsPerAccount()
        { 
            Account = g.Key,
            Holding = g.Sum(e => e.Value) + accountsResult.Value!.Where(x => x.AccountId == g.Key.AccountId).Select(a => a.Cash).First()
        }).ToList();

        return Result<List<HoldingsPerAccount>>.Success(accountHoldings);
    }
}