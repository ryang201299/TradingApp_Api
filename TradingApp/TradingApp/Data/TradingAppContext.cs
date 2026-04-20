using Microsoft.EntityFrameworkCore;
using TradingApp.Models.Database;

namespace TradingApp.Data;

public class TradingAppContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }

    public DbSet<Security> Securities { get; set; }

    public DbSet<SecurityPrice> SecurityPrices { get; set; }

    public DbSet<TransactionType> TransactionTypes { get; set; }

    public DbSet<Transaction> Transactions { get; set; }

    public TradingAppContext(DbContextOptions<TradingAppContext> options) : base(options) { }
}