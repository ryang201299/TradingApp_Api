using TradingApp.Models.Database;
using TradingApp.Models.DTO.Request;

namespace TradingApp.Models.Interfaces.ControllerHelpers;

public interface ITransactionControllerHelper
{
    /// <summary>
    /// Retrieves all transactions across all accounts
    /// </summary>
    /// <returns>All transactions across all accounts</returns>
    Task<Result<List<Transaction>>> GetTransactionsAsync();

    /// <summary>
    /// Retrieves all transactions across a single account
    /// </summary>
    /// <param name="accountId">Account Id</param>
    /// <returns>All transactions across a single account</returns>
    Task<Result<List<Transaction>>> GetTransactionsAsync(int accountId);

    /// <summary>
    /// Purchase shares in a security
    /// </summary>
    /// <param name="request">Request information including quantity and security being purchased</param>
    Task<Result> BuySecurityAsync(TransactionRequestDto request);

    /// <summary>
    /// Sell shares in a security
    /// </summary>
    /// <param name="request">Request information including quantity and security being sold</param>
    Task<Result> SellSecurityAsync(TransactionRequestDto request);
}
