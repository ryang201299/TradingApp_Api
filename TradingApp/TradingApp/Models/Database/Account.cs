using System.ComponentModel.DataAnnotations;

namespace TradingApp.Models.Database;

public class Account
{
    [Key]
    public int AccountId { get; set; }

    [MaxLength(50)]
    public required string Name { get; set; }

    public ICollection<Transaction>? Transactions { get; set; }
}