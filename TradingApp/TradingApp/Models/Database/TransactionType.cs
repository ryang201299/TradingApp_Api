using System.ComponentModel.DataAnnotations;

namespace TradingApp.Models.Database;

public class TransactionType
{
    [Key]
    public int TransactionTypeId { get; set; }

    [MaxLength(25)]
    public required string TypeDescription { get; set; }
}