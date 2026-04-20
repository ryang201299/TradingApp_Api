using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.Interfaces;
using TradingApp.Models.Interfaces.ControllerHelpers;

namespace TradingApp.Helpers.Controllers;

public class SecurityControllerHelper : ISecurityControllerHelper
{
    private readonly ILogger<SecurityControllerHelper> _logger;
    private readonly ISecurityService _securityService;

    public SecurityControllerHelper(ILogger<SecurityControllerHelper> logger, ISecurityService securityService)
    {
        _logger = logger;
        _securityService = securityService;
    }

    public async Task<Result<List<Security>>> GetSecuritiesAsync()
    {
        return await _securityService.GetSecuritiesAsync();
    }

    public async Task<Result<int?>> GetSecurityIdAsync(string name)
    {
        return await _securityService.GetSecurityIdAsync(name);
    }
}
