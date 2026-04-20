using System.ComponentModel.DataAnnotations;

namespace TradingApp.Models.DTO.Response;

public class SecurityDto
{
    [Required]
    public int SecurityId { get; set; }

    [Required]
    public required string SecurityName { get; set; }
}
