using Microsoft.EntityFrameworkCore;
using TradingApp.Data;
using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.Interfaces;

namespace TradingApp.Helpers.Services;

public class SecurityPriceService : ISecurityPriceService
{
    private readonly ILogger<SecurityPriceService> _logger;
    private readonly TradingAppContext _context;

    public SecurityPriceService(ILogger<SecurityPriceService> logger, TradingAppContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <inheritdoc />
    public async Task<Result<List<SecurityPrice>>> GetSecurityPricesAsync()
    {
        try
        {
            _logger.LogInformation("Retrieving security prices.");
            List<SecurityPrice> securityPrices = await _context.SecurityPrices
                .Include(x => x.Security)
                .ToListAsync();

            _logger.LogInformation("Retrieved price for `{SecurityCount}` securities.", securityPrices.Count);
            return Result<List<SecurityPrice>>.Success(securityPrices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve prices.");
            return Result<List<SecurityPrice>>.Failure(ex.Message);
        }
    }

    /// <inheritdoc />
    public async Task<Result<SecurityPrice>> GetSecurityPriceAsync(int securityId)
    {
        try
        {
            _logger.LogInformation("Retrieving latest price for security `{SecurityId}`.", securityId);

            SecurityPrice? securityPrice = await _context.SecurityPrices
                .Where(x => x.SecurityId == securityId)
                .FirstOrDefaultAsync();

            _logger.LogInformation("Latest price is `{Price}`.", securityPrice);

            if (securityPrice == null) { return Result<SecurityPrice>.Failure("No price found."); }

            return Result<SecurityPrice>.Success(securityPrice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve price for security `{SecurityId}`.", securityId);
            return Result<SecurityPrice>.Failure(ex.Message);
        }
    }
}
