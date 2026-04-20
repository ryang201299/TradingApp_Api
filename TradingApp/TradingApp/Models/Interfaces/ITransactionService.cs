using TradingApp.Models.Database;

namespace TradingApp.Models.Interfaces;

public interface ITransactionService
{
    Task<Result<List<Transaction>>> GetTransactionsAsync(int accountId);

    Task<Result> BuySecurityAsync(Transaction transaction);

    Task<Result> SellSecurityAsync(Transaction transaction);
}