using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingApp.Models.Database;

public class Transaction
{
    [Key]
    public int TransactionId { get; set; }

    public required Account Account { get; set; }

    public required TransactionType TransactionType { get; set; }

    public required Security Security { get; set; }

    [Column(TypeName = "decimal(11,2)")]
    public decimal SecurityPrice { get; set; }

    public int Quantity { get; set; }
}