using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.Enums;

namespace TradingApp.Helpers.Services;

public interface ITransactionTypeService
{
    Task<Result<TransactionType>> GetTransactionTypeAsync(TransactionTypeEnum transactionType);
}
