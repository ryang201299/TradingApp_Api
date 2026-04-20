using Microsoft.AspNetCore.Mvc;
using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.DTO.Request;
using TradingApp.Models.DTO.Response;
using TradingApp.Models.Interfaces.ControllerHelpers;

namespace TradingApp.Controllers;

[ApiController]
[Route("api/transactions")]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionController> _logger;
    private readonly ITransactionControllerHelper _transactionHelper;

    public TransactionController(ILogger<TransactionController> logger, ITransactionControllerHelper transactionHelper)
    {
        _logger = logger;
        _transactionHelper = transactionHelper;
    }

    [HttpPost("buy")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> BuySecurity([FromBody] TransactionRequestDto request)
    {
        try
        {
            Result tradeResponse = await _transactionHelper.BuySecurityAsync(request);

            if (!tradeResponse.IsSuccess)
            {
                return BadRequest(tradeResponse.Error);
            }

            return Ok(tradeResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred " +
                "generating a purchase against security `{Securityid}` " +
                "for account `{AccountId}`.", request.SecurityId, request.AccountId);
            return BadRequest(ex);
        }
    }

    [HttpPost("sell")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SellSecurity([FromBody] TransactionRequestDto request)
    {
        try
        {
            Result tradeResponse = await _transactionHelper.SellSecurityAsync(request);

            if (!tradeResponse.IsSuccess)
            {
                return BadRequest(tradeResponse.Error);
            }

            return Ok(tradeResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred " +
                "generating a sell against security `{Securityid}` " +
                "for account `{AccountId}`.", request.SecurityId, request.AccountId);
            return BadRequest(ex);
        }
    }

    [HttpGet("{accountId:int}")]
    [ProducesResponseType(typeof(List<TransactionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactions(int accountId)
    {
        try
        {
            Result<List<Transaction>> transactionsResult = await _transactionHelper.GetTransactionsAsync(accountId);

            if (!transactionsResult.IsSuccess)
            {
                return BadRequest(transactionsResult.Error);
            }

            List<TransactionDto> transactions = transactionsResult.Value!.Select(x => new TransactionDto()
            {
                TransactionId = x.TransactionId,
                Account = new AccountDto() { AccountId = x.Account.AccountId, Name = x.Account.Name },
                Security = new SecurityDto() { SecurityId = x.Security.SecurityId, SecurityName = x.Security.SecurityName },
                SecurityPrice = x.SecurityPrice,
                Quantity = x.Quantity
            }).ToList();

            return Ok(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred retrieving transactions for account `{AccountId}`.", accountId);
            return BadRequest(ex);
        }
    }
}
