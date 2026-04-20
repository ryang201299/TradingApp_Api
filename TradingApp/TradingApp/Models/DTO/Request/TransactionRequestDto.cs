using System.ComponentModel.DataAnnotations;

namespace TradingApp.Models.DTO.Request;

public class TransactionRequestDto
{
    [Required]
    public int AccountId { get; set; }

    [Required]
    public int SecurityId { get; set; }
}
