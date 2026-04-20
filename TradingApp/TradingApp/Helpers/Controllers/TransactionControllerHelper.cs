using TradingApp.Helpers.Services;
using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.DTO.Request;
using TradingApp.Models.Enums;
using TradingApp.Models.Interfaces;
using TradingApp.Models.Interfaces.ControllerHelpers;

namespace TradingApp.Helpers.Controllers;

public class TransactionControllerHelper : ITransactionControllerHelper
{
    private readonly ITransactionService _transactionService;
    private readonly IAccountService _accountService;
    private readonly ITransactionTypeService _transactionTypeService;
    private readonly ISecurityService _securityService;
    private readonly ISecurityPriceService _securityPriceService;

    public TransactionControllerHelper(
        ITransactionService transactionService,
        IAccountService accountService,
        ITransactionTypeService transactionTypeService,
        ISecurityService securityService,
        ISecurityPriceService securityPriceService)
    {
        _transactionService = transactionService;
        _accountService = accountService;
        _transactionTypeService = transactionTypeService;
        _securityService = securityService;
        _securityPriceService = securityPriceService;
    }

    public async Task<Result<List<Transaction>>> GetTransactionsAsync(int accountId)
    {
        return await _transactionService.GetTransactionsAsync(accountId);
    }

    public async Task<Result> BuySecurityAsync(TransactionRequestDto request)
    {
        Result<Account?> account = await _accountService.GetAccountAsync(request.AccountId);
        Result<Security?> security = await _securityService.GetSecurityAsync(request.SecurityId);
        Result<TransactionType?> transactionType = await _transactionTypeService.GetTransactionTypeAsync(TransactionTypeEnum.BUY);
        Result<SecurityPrice?> securityPrice = await _securityPriceService.GetSecurityPriceAsync(request.SecurityId);

        if (account.Value is null || security.Value is null || transactionType.Value is null || securityPrice.Value is null)
        {
            return Result.Failure("Failed to retrieve one of the required: account, security, security price, or transaction type.");
        }

        Transaction transaction = new()
        {
            Account = account.Value,
            Security = security.Value,
            TransactionType = transactionType.Value,
            SecurityPrice = securityPrice.Value.Price,
            Quantity = 1
        };

        return await _transactionService.BuySecurityAsync(transaction);
    }

    public async Task<Result> SellSecurityAsync(TransactionRequestDto request)
    {
        Result<Account?> account = await _accountService.GetAccountAsync(request.AccountId);
        Result<Security?> security = await _securityService.GetSecurityAsync(request.SecurityId);
        Result<TransactionType?> transactionType = await _transactionTypeService.GetTransactionTypeAsync(TransactionTypeEnum.SELL);
        Result<SecurityPrice?> securityPrice = await _securityPriceService.GetSecurityPriceAsync(request.SecurityId);

        if (account.Value is null || security.Value is null || transactionType.Value is null || securityPrice.Value is null)
        {
            return Result.Failure("Failed to retrieve one of the required: account, security, security price, or transaction type.");
        }

        Transaction transaction = new()
        {
            Account = account.Value,
            Security = security.Value,
            TransactionType = transactionType.Value,
            SecurityPrice = securityPrice.Value.Price,
            Quantity = 1
        };

        return await _transactionService.SellSecurityAsync(transaction);
    }
}
