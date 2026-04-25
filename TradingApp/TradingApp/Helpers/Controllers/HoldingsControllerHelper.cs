using System.Text.RegularExpressions;
using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.DTO.Response;
using TradingApp.Models.Interfaces;
using TradingApp.Models.Interfaces.ControllerHelpers;

namespace TradingApp.Helpers.Controllers;

public class HoldingsControllerHelper : IHoldingsControllerHelper
{
    private readonly ITransactionService _transactionService;
    private readonly ISecurityPriceService _securityPriceService;
    private readonly IAccountService _accountService;

    public HoldingsControllerHelper(ITransactionService transactionService, ISecurityPriceService securityPriceService, IAccountService accountService)
    {
        _transactionService = transactionService;
        _securityPriceService = securityPriceService;
        _accountService = accountService;
    }

    private async Task<List<HoldingQuantity>> GetHoldingQuantities()
    {
        // get all transactions, joined with latest prices

        Result<List<Transaction>> transactionsResult = await _transactionService.GetTransactionsAsync();
        List<Transaction> transactions = transactionsResult.Value;

        // group by account and security, and sum of quantity (subtract on sell, add on buy)
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

        return holdingQuantities;
    }

    private async Task<List<HoldingValue>> GetHoldingValue(List<HoldingQuantity> holdingQuantities)
    {
        Result<List<SecurityPrice>> securityPricesResult = await _securityPriceService.GetSecurityPricesAsync();
        List<SecurityPrice> securityPrices = securityPricesResult.Value;

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

        return holdingValue;
    }

    // TODO: Add result pattern later
    public async Task<List<HoldingsPerAccount>> GetHoldingsAsync()
    {
        List<HoldingQuantity> holdingQuantities = await GetHoldingQuantities();
        List<HoldingValue> accountHoldingsNotZero = await GetHoldingValue(holdingQuantities);

        // get accounts cash
        Result<List<Account>> accountsResult = await _accountService.GetAccountsAsync();

        // group by account and sum of holdings
        var accountHoldings = accountHoldingsNotZero.GroupBy(x => x.Account).Select(g => new HoldingsPerAccount()
        { 
            Account = g.Key,
            Holding = g.Sum(e => e.Value) + accountsResult.Value.Where(x => x.AccountId == g.Key.AccountId).Select(a => a.Cash).First()
        }).ToList();

        return accountHoldings;
    }
}