using TradingApp.Models;
using TradingApp.Models.Database;
using TradingApp.Models.Interfaces;
using TradingApp.Models.Interfaces.ControllerHelpers;

namespace TradingApp.Helpers.Controllers;

public class SecurityPricesControllerHelper : ISecurityPricesControllerHelper
{
    private readonly ISecurityPriceService _securityPriceService;

    public SecurityPricesControllerHelper(ISecurityPriceService securityPriceService)
    {
        _securityPriceService = securityPriceService;
    }

    public async Task<Result<List<SecurityPrice>>> GetSecurityPricesAsync()
    {
        return await _securityPriceService.GetSecurityPricesAsync();
    }
}
