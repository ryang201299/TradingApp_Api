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
    [HttpGet]
    [ProducesResponseType(typeof(List<HoldingsPerAccountDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHoldings()
    {
        try
        {
            Result<List<HoldingsPerAccount>> holdingsResult = await _holdingsHelper.GetHoldingsAsync();

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
}