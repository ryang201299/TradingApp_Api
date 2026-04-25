using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.Interfaces;
using TradingApp.Models.Interfaces.ControllerHelpers;

namespace TradingApp.Helpers.Controllers;
public class PerformanceControllerHelper : IPerformanceControllerHelper
{
    private readonly ITransactionService _transactionService;
    private readonly ISecurityPriceService _securityPriceService;

    public PerformanceControllerHelper(ITransactionService transactionService, ISecurityPriceService securityPriceService)
    { 
        _transactionService = transactionService;
        _securityPriceService = securityPriceService;
    }

    // TODO: Add reuslt pattern
    public async Task<List<AccountUnrealisedReturns>> GetUnrealisedReturns()
    {
        Result<List<Transaction>> transactions = await _transactionService.GetTransactionsAsync();

        // get total value of all held quantities at original price
        var shareCost = transactions.Value
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
            .Select(g => new
            {
                Account = g.Key,
                TotalShareCost = g.Sum(e => e.TotalHeld * e.Price)
            }).ToList();

        // get total value of all held qunaities at latest price
        Result<List<SecurityPrice>> securityPrices = await _securityPriceService.GetSecurityPricesAsync();

        var holdingsPerAccount = transactions.Value
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
                securityPrice in securityPrices.Value
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
            .Select(g => new
            {
                Account = g.Key,
                TotalShareWorth = g.Sum(e => e.Held * e.Price)
            }).ToList();

        // (new - old) / old * 100
        List<AccountUnrealisedReturns> unrealisedReturns = shareWorthByAccount
            .Select(x => new AccountUnrealisedReturns()
            {
                Account = x.Account,
                UnrealisedReturns =
                (
                    (x.TotalShareWorth - shareCostPerAccount.FirstOrDefault(e => e.Account == x.Account).TotalShareCost)
                    / shareCostPerAccount.FirstOrDefault(e => e.Account == x.Account).TotalShareCost
                ) * 100
            }).ToList();

        return unrealisedReturns;
    }
}
