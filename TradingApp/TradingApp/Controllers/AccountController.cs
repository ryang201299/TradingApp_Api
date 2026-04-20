using Microsoft.AspNetCore.Mvc;
using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.DTO.Response;
using TradingApp.Models.Interfaces;

namespace TradingApp.Controllers;

/// <summary>
/// Controller housing all endpoints relating to Accounts
/// </summary>
[ApiController]
[Route("api/accounts")]
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
    [ProducesResponseType(typeof(List<AccountDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccounts()
    {
        try
        {
            Result<List<Account>> accountsResult = await _accountHelper.GetAccountsAsync();

            if (!accountsResult.IsSuccess)
            {
                return BadRequest(accountsResult.Error);
            }

            return Ok(accountsResult.Value!.Select(x => new List<AccountDto>
            {
                new AccountDto()
                {
                    AccountId = x.AccountId,
                    Name = x.Name
                }
            }).ToList());
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
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccount(int id)
    {
        try
        {
            Result<Account?> accountResult = await _accountHelper.GetAccountAsync(id);

            if (!accountResult.IsSuccess)
            {
                return BadRequest(accountResult.Error);
            }

            if (accountResult.Value == null)
            {
                return NotFound();
            }

            return Ok(new AccountDto()
            {
                AccountId = accountResult.Value.AccountId,
                Name = accountResult.Value.Name
            });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving accounts.");
            return BadRequest(ex);
        }
    }

    [HttpGet("{name}/id")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAccountId(string name)
    {
        try
        {
            Result<int?> accountId = await _accountHelper.GetAccountIdAsync(name);

            if (!accountId.IsSuccess)
            {
                return BadRequest(accountId.Error);
            }

            if (accountId.Value == null)
            {
                return NotFound();
            }

            return Ok(accountId.Value);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving account id.");
            return BadRequest(ex);
        }
    }
}