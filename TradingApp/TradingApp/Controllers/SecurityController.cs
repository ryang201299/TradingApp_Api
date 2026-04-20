using Microsoft.AspNetCore.Mvc;
using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.DTO.Response;
using TradingApp.Models.Interfaces.ControllerHelpers;

namespace TradingApp.Controllers;

[ApiController]
[Route("api/securities")]
public class SecurityController : ControllerBase
{
    private readonly ILogger<SecurityController> _logger;
    private readonly ISecurityControllerHelper _securityHelper;

    public SecurityController(ILogger<SecurityController> logger, ISecurityControllerHelper securityHelper)
    {
        _logger = logger;
        _securityHelper = securityHelper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<SecurityDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSecurities()
    {
        try
        {
            Result<List<Security>> securitiesResult = await _securityHelper.GetSecuritiesAsync();

            if (!securitiesResult.IsSuccess)
            {
                return BadRequest(securitiesResult.Error);
            }

            List<SecurityDto> securities = securitiesResult.Value!.Select(x => new SecurityDto()
            {
                SecurityId = x.SecurityId,
                SecurityName = x.SecurityName
            }).ToList();

            return Ok(securities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred retrieving securities.");
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{name}/id")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSecurityId(string name)
    {
        try
        {
            Result<int?> securityId = await _securityHelper.GetSecurityIdAsync(name);

            if (!securityId.IsSuccess)
            {
                return BadRequest(securityId.Error);
            }

            if (securityId.Value == null)
            {
                return NotFound();
            }

            return Ok(securityId.Value);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving account id.");
            return BadRequest(ex);
        }
    }
}
