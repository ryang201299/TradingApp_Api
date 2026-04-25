namespace TradingApp.Models.DTO.Response;
public class UnrealisedReturnsDto
{
    public required AccountDto Account { get; set; }

    public required decimal UnrealisedReturns { get; set; }
}