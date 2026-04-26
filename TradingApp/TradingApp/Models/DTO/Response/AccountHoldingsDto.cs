namespace TradingApp.Models.DTO.Response;
public class AccountHoldingsDto
{
    public required AccountDto Account { get; set; }

    public required List<SecurityHolding> Holdings { get; set; }
}