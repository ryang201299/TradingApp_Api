using Microsoft.AspNetCore.Mvc;
using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.DTO.Response;
using TradingApp.Models.Interfaces.ControllerHelpers;

namespace TradingApp.Controllers
{
    [Route("api/securityprices")]
    [ApiController]
    public class SecurityPricesController : ControllerBase
    {
        private readonly ILogger<SecurityPricesController> _logger;
        private readonly ISecurityPricesControllerHelper _securityPricesHelper;

        public SecurityPricesController(ILogger<SecurityPricesController> logger, ISecurityPricesControllerHelper securityPricesHelper)
        {
            _logger = logger;
            _securityPricesHelper = securityPricesHelper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<SecurityPriceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSecurityPrices()
        {
            try
            {
                Result<List<SecurityPrice>> securityPricesResult = await _securityPricesHelper.GetSecurityPricesAsync();

                if (!securityPricesResult.IsSuccess)
                {
                    return BadRequest(securityPricesResult.Error);
                }

                return Ok(securityPricesResult.Value!.Select(x => new SecurityPriceDto()
                {
                    SecurityId = x.SecurityId,
                    Price = x.Price
                }).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred retrieving security prices.");
                return BadRequest(ex);
            }
        }
    }
}
