using Microsoft.EntityFrameworkCore;
using TradingApp.Data;
using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.Interfaces;

namespace TradingApp.Helpers.Services;

public class SecurityService : ISecurityService
{
    private readonly ILogger<SecurityService> _logger;
    private readonly TradingAppContext _context;

    public SecurityService(ILogger<SecurityService> logger, TradingAppContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<List<Security>>> GetSecuritiesAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving securities.");
            List<Security> securities = await _context.Securities.ToListAsync();

            _logger.LogInformation("Retrieved `{SecurityCount}` securities.", securities.Count);
            return Result<List<Security>>.Success(securities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve securities.");
            return Result<List<Security>>.Failure(ex.Message);
        }
    }

    public async Task<Result<int>> GetSecurityIdAsync(string securityName)
    {
        try
        {
            _logger.LogInformation("Retrieving security `{SecurityName}`.", securityName);

            int? securityId = await _context.Securities
                .Where(x => x.SecurityName == securityName)
                .Select(x => x.SecurityId).FirstOrDefaultAsync();

            if (securityId == null)
            {
                _logger.LogInformation("Security `{SecurityName}` not found.", securityName);
                return Result<int>.Failure("Security not found.");
            }

            _logger.LogInformation("Retrieved security id `{SecurityId}` from name `{SecurityName}`.", securityId, securityName);

            return Result<int>.Success((int)securityId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving security Id given name `{Name}`.", securityName);
            return Result<int>.Failure(ex.Message);
        }
    }

    public async Task<Result<Security>> GetSecurityAsync(int id)
    {
        try
        {
            _logger.LogInformation("Getting security `{SecurityId}.`", id);

            Security? security = await _context.Securities
                .Where(x => x.SecurityId == id)
                .FirstOrDefaultAsync();

            if (security == null)
            {
                _logger.LogError("Security not found.");
                return Result<Security>.Failure("Security not found.");
            }

            return Result<Security>.Success(security);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving security `{SecurityId}`.", id);
            return Result<Security>.Failure(ex.Message);
        }
    }
}
