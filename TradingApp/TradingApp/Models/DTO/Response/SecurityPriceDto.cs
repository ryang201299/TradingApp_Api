using System.ComponentModel.DataAnnotations;

namespace TradingApp.Models.DTO.Response;

public class SecurityPriceDto
{
    [Required]
    public int SecurityId { get; set; }

    [Required]
    public decimal Price { get; set; }
}
