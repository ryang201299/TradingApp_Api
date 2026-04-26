using Microsoft.AspNetCore.Mvc;
using TradingApp.Models;
using TradingApp.Models.DTO.Response;
using TradingApp.Models.Interfaces.ControllerHelpers;

namespace TradingApp.Controllers
{
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
                Result<List<AccountUnrealisedReturns>> unrealisedReturns = await _performanceHelper.GetUnrealisedReturns();

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
    }
}
