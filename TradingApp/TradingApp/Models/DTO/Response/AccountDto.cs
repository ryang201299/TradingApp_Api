using System.ComponentModel.DataAnnotations;

namespace TradingApp.Models.DTO.Response
{
    public class AccountDto
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        public required string Name { get; set; }
    }
}
