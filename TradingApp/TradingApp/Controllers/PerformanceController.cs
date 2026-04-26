using Microsoft.AspNetCore.Mvc;
using TradingApp.Models;
using TradingApp.Models.DTO.Response;
using TradingApp.Models.Interfaces.ControllerHelpers;

namespace TradingApp.Controllers;

[Route("api/performance")]
[ApiController]
public class PerformanceController : ControllerBase
{
    private readonly IPerformanceControllerHelper _performanceHelper;

    public PerformanceController(IPerformanceControllerHelper performanceHelper)
    { 
        _performanceHelper = performanceHelper;
    }

    [HttpGet("unrealisedreturns")]
    [ProducesResponseType(typeof(List<UnrealisedReturnsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUnrealisedReturns()
    {
        try
        {
            Result<List<AccountUnrealisedReturns>> unrealisedReturns = await _performanceHelper.GetUnrealisedReturnsAsync();

            if (!unrealisedReturns.IsSuccess)
            {
                return BadRequest(unrealisedReturns.Error);
            }

            return Ok(unrealisedReturns.Value!.Select(x => new UnrealisedReturnsDto()
            { 
                Account = new AccountDto() { AccountId = x.Account.AccountId, Name = x.Account.Name, Cash = x.Account.Cash },
                UnrealisedReturns = x.UnrealisedReturns
            }).ToList());
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpGet("unrealisedreturns/{id:int}")]
    [ProducesResponseType(typeof(UnrealisedReturnsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUnrealisedReturnsForAccount(int id)
    {
        try
        {
            Result<AccountUnrealisedReturns> unrealisedReturnsResult = await _performanceHelper.GetUnrealisedReturnsForAccountAsync(id);

            if (!unrealisedReturnsResult.IsSuccess)
            {
                return BadRequest(unrealisedReturnsResult.Error);
            }

            return Ok(new UnrealisedReturnsDto()
            { 
                Account = new AccountDto()
                { 
                    AccountId = unrealisedReturnsResult.Value!.Account.AccountId,
                    Name = unrealisedReturnsResult.Value.Account.Name,
                    Cash = unrealisedReturnsResult.Value.Account.Cash
                },
                UnrealisedReturns = unrealisedReturnsResult.Value.UnrealisedReturns
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
