using Microsoft.AspNetCore.Mvc;
using TradingApp.Models;
using TradingApp.Models.DTO.Response;
using TradingApp.Models.Interfaces.ControllerHelpers;

namespace TradingApp.Controllers;

[Route("api/holdings")]
[ApiController]
public class HoldingsController : ControllerBase
{
    private readonly ILogger<HoldingsController> _logger;
    private readonly IHoldingsControllerHelper _holdingsHelper;

    public HoldingsController(ILogger<HoldingsController> logger, IHoldingsControllerHelper holdingsHelper)
    {
        _logger = logger;
        _holdingsHelper = holdingsHelper;
    }

    /// <summary>
    /// Gets total holdings for all accounts
    /// </summary>
    /// <returns>Total holdings for all accounts</returns>
    [HttpGet("overall")]
    [ProducesResponseType(typeof(List<HoldingsPerAccountDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOverallHoldings()
    {
        try
        {
            Result<List<AccountHolding>> holdingsResult = await _holdingsHelper.GetOverallHoldingsForAllAccountsAsync();

            if (!holdingsResult.IsSuccess)
            {
                return BadRequest(holdingsResult.Error);
            }

            return Ok(holdingsResult.Value!.Select(x => new HoldingsPerAccountDto()
            { 
                Account = new AccountDto() { AccountId = x.Account.AccountId, Name = x.Account.Name, Cash = x.Account.Cash },
                Holding = x.Holding,
            }).ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred retrieving holdings.");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Gets total holdings for an account
    /// </summary>
    /// <returns>Total holdings for an accounts</returns>
    [HttpGet("overall/{id:int}")]
    [ProducesResponseType(typeof(HoldingsPerAccountDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOverallHoldingsForAccount(int id)
    {
        try
        {
            Result<AccountHolding> holdingsResult = await _holdingsHelper.GetOverallHoldingsForAnAccountAsync(id);

            if (!holdingsResult.IsSuccess)
            {
                return BadRequest(holdingsResult.Error);
            }

            return Ok(new HoldingsPerAccountDto()
            {
                Account = new AccountDto() { AccountId = holdingsResult.Value!.Account.AccountId, Name = holdingsResult.Value.Account.Name, Cash = holdingsResult.Value.Account.Cash },
                Holding = holdingsResult.Value.Holding
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred retrieving holdings.");
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Gets total holdings for an account
    /// </summary>
    /// <returns>Total holdings for an accounts</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AccountHoldingsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHoldingsForAccount(int id)
    {
        try
        {
            Result<AccountHoldings> holdingsResult = await _holdingsHelper.GetHoldingsForAnAccountAsync(id);

            if (!holdingsResult.IsSuccess)
            {
                return BadRequest(holdingsResult.Error);
            }

            return Ok(new AccountHoldingsDto() 
            { 
                Account = new AccountDto() 
                { 
                    AccountId = holdingsResult.Value!.Account.AccountId,
                    Name = holdingsResult.Value.Account.Name,
                    Cash = holdingsResult.Value.Account.Cash
                },
                Holdings = holdingsResult.Value.Holdings
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred retrieving holdings.");
            return BadRequest(ex.Message);
        }
    }
}