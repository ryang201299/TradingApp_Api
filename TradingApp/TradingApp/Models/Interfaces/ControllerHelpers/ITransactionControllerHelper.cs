using TradingApp.Models.Database;
using TradingApp.Models.DTO.Request;

namespace TradingApp.Models.Interfaces.ControllerHelpers;

public interface ITransactionControllerHelper
{
    Task<Result<List<Transaction>>> GetTransactionsAsync(int accountId);

    Task<Result> BuySecurityAsync(TransactionRequestDto request);

    Task<Result> SellSecurityAsync(TransactionRequestDto request);
}
