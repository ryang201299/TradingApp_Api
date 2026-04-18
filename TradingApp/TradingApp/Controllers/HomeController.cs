using Microsoft.AspNetCore.Mvc;
using TradingApp.Models;
using TradingApp.Models.DTO;
using TradingApp.Models.Interfaces;

namespace TradingApp.Controllers;

/// <summary>
/// Controller housing all endpoints relating to Accounts
/// </summary>
[ApiController]
[Route("accounts")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountControllerHelper _accountHelper;

    public AccountController(ILogger<AccountController> logger, IAccountControllerHelper accountHelper)
    {
        _logger = logger;
        _accountHelper = accountHelper;
    }

    /// <summary>
    /// Retrieves information about accounts
    /// </summary>
    /// <returns>List of accounts</returns>
    [HttpGet]
    public async Task<IActionResult> GetAccounts()
    {
        try
        {
            Result<List<AccountDto>> accountsResult = await _accountHelper.GetAccountsAsync();

            if (!accountsResult.IsSuccess)
            {
                return BadRequest(accountsResult.Error);

            }

            return Ok(accountsResult.Value);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving accounts.");
            return BadRequest(ex);
        }
    }

    /// <summary>
    /// Retrieves information about a single account
    /// </summary>
    /// <param name="id">Account Id</param>
    /// <returns>Information about an account</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccount(int id)
    {
        try
        {
            Result<AccountDto?> accountResult = await _accountHelper.GetAccountAsync(id);

            if(!accountResult.IsSuccess)
            {
                return BadRequest(accountResult.Error);
            }

            if (accountResult.Value == null)
            {
                return NotFound();
            }

            return Ok(accountResult.Value);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving accounts.");
            return BadRequest(ex);
        }
    }
}