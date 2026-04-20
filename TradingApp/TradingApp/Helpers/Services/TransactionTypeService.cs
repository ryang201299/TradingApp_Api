using Microsoft.EntityFrameworkCore;
using TradingApp.Data;
using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.Enums;

namespace TradingApp.Helpers.Services;

public class TransactionTypeService : ITransactionTypeService
{
    private readonly ILogger<TransactionTypeService> _logger;
    private readonly TradingAppContext _context;

    public TransactionTypeService(ILogger<TransactionTypeService> logger, TradingAppContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<TransactionType?>> GetTransactionTypeAsync(TransactionTypeEnum transactionTypeEnum)
    {
        try
        {
            TransactionType? transactionType = await _context.TransactionTypes
                .Where(x => x.TransactionTypeId == (int)transactionTypeEnum)
                .FirstOrDefaultAsync();

            return Result<TransactionType?>.Success(transactionType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving transaction type `{TransactionTypeId}`.", (int)transactionTypeEnum);
            return Result<TransactionType?>.Failure(ex.Message);
        }
    }
}
