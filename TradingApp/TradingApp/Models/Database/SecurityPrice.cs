using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingApp.Models.Database;

public class SecurityPrice
{
    [Key, ForeignKey(nameof(Security))]
    public int SecurityId { get; set; }

    public required Security Security { get; set; }

    [Column(TypeName = "decimal(11, 2)")]
    public decimal Price { get; set; }
}