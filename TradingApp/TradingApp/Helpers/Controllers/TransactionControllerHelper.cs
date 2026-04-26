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
    private readonly ILogger<TransactionControllerHelper> _logger;
    private readonly ITransactionService _transactionService;
    private readonly IAccountService _accountService;
    private readonly ITransactionTypeService _transactionTypeService;
    private readonly ISecurityService _securityService;
    private readonly ISecurityPriceService _securityPriceService;

    public TransactionControllerHelper(
        ILogger<TransactionControllerHelper> logger,
        ITransactionService transactionService,
        IAccountService accountService,
        ITransactionTypeService transactionTypeService,
        ISecurityService securityService,
        ISecurityPriceService securityPriceService)
    {
        _logger = logger;
        _transactionService = transactionService;
        _accountService = accountService;
        _transactionTypeService = transactionTypeService;
        _securityService = securityService;
        _securityPriceService = securityPriceService;
    }

    /// <inheritdoc />
    public async Task<Result<List<Transaction>>> GetTransactionsAsync()
    {
        return await _transactionService.GetTransactionsAsync();
    }

    /// <inheritdoc />
    public async Task<Result<List<Transaction>>> GetTransactionsAsync(int accountId)
    {
        return await _transactionService.GetTransactionsAsync(accountId);
    }

    /// <inheritdoc />
    public async Task<Result> BuySecurityAsync(TransactionRequestDto request)
    {
        _logger.LogInformation("Purchasing `{Quantity}` of shares in `{Security}`.", request.Quantity, request.SecurityId);

        // Retrieve required data
        Result<Account> account = await _accountService.GetAccountAsync(request.AccountId);
        Result<Security> security = await _securityService.GetSecurityAsync(request.SecurityId);
        Result<TransactionType> transactionType = await _transactionTypeService.GetTransactionTypeAsync(TransactionTypeEnum.BUY);
        Result<SecurityPrice> securityPrice = await _securityPriceService.GetSecurityPriceAsync(request.SecurityId);

        if (!account.IsSuccess || !security.IsSuccess || !transactionType.IsSuccess || !securityPrice.IsSuccess)
        {
            return Result.Failure("Failed to retrieve one of the required: account, security, security price, or transaction type.");
        }

        // Widthdraw required cash for transaction
        Result widthdrawalResult = await _accountService.WidthdrawCashAsync(account.Value!, securityPrice.Value!.Price * request.Quantity);

        if (!widthdrawalResult.IsSuccess)
        {
            return Result.Failure(widthdrawalResult.Error!);
        }

        // Purchase shares
        Transaction transaction = new()
        {
            Account = account.Value!,
            Security = security.Value!,
            TransactionType = transactionType.Value!,
            SecurityPrice = securityPrice.Value.Price,
            Quantity = request.Quantity
        };

        Result buyResult = await _transactionService.BuySecurityAsync(transaction);

        if (!buyResult.IsSuccess)
        {
            // Refund cash if the purchase was unsuccessful
            Result refundCashResult = await _accountService.AddCashAsync(account.Value!, securityPrice.Value.Price * request.Quantity);

            if (!refundCashResult.IsSuccess) { return Result.Failure("Purchase of shares failed, but cash failed to be refunded to the account."); }
        }

        return Result.Success();
    }

    /// <inheritdoc />
    public async Task<Result> SellSecurityAsync(TransactionRequestDto request)
    {
        _logger.LogInformation("Selling `{Quantity}` shares in `{Security}`.", request.Quantity, request.SecurityId);

        // Retrieve required data for transaction
        Result<Account> account = await _accountService.GetAccountAsync(request.AccountId);
        Result<Security> security = await _securityService.GetSecurityAsync(request.SecurityId);
        Result<TransactionType> transactionType = await _transactionTypeService.GetTransactionTypeAsync(TransactionTypeEnum.SELL);
        Result<SecurityPrice> securityPrice = await _securityPriceService.GetSecurityPriceAsync(request.SecurityId);

        if (!account.IsSuccess || !security.IsSuccess || !transactionType.IsSuccess || !securityPrice.IsSuccess)
        {
            return Result.Failure("Failed to retrieve one of the required: account, security, security price, or transaction type.");
        }

        // Check account has enough shares to complete requested transaction
        Result<List<Transaction>> accountTransactions = await _transactionService.GetTransactionsAsync(request.AccountId);

        if (!accountTransactions.IsSuccess) 
        { 
            return Result.Failure("Failed to check account transactions. Cannot continue with sale."); 
        }

        // Retrieve all transactions for this account
        List<Transaction> relevantTransactions = accountTransactions.Value!.Where(x => x.Security.SecurityId == request.SecurityId).ToList();

        // Calculate number of held shares in this security
        int countOfPurchasedShares = relevantTransactions.Where(x => x.TransactionType.TransactionTypeId == 1).Sum(t => t.Quantity);
        int countOfSoldShares = relevantTransactions.Where(x => x.TransactionType.TransactionTypeId == 2).Sum(t => t.Quantity);
        int securitySharesHeld = countOfPurchasedShares - countOfSoldShares;

        if (securitySharesHeld < request.Quantity)
        {
            return Result.Failure("Insufficient shares held to complete sale.");
        }

        // Complete transaction
        Transaction transaction = new()
        {
            Account = account.Value!,
            Security = security.Value!,
            TransactionType = transactionType.Value!,
            SecurityPrice = securityPrice.Value!.Price,
            Quantity = request.Quantity
        };

        Result sellResult = await _transactionService.SellSecurityAsync(transaction);

        if (!sellResult.IsSuccess) 
        {
            return Result.Failure("Failed to sell shares.");
        }

        // Add proceeds to account cash balance
        Result addCashResult = await _accountService.AddCashAsync(account.Value!, request.Quantity * securityPrice.Value.Price);

        if (!addCashResult.IsSuccess)
        {
            return Result.Failure("Sale of security was successful, but funds failed to be returned to the account.");
        }

        return Result.Success();
    }
}
