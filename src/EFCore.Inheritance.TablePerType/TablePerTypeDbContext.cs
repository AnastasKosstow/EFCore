using EFCore.Inheritance.TablePerType.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Inheritance.TablePerType;

public class TablePerTypeDbContext : DbContext
{
    public TablePerTypeDbContext(DbContextOptions<TablePerTypeDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = default;
    public DbSet<BillingDetail> BillingDetails { get; set; } = default;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccount>().ToTable("BankAccounts");
        modelBuilder.Entity<CreditCard>().ToTable("CreditCards");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TablePerTypeDbContext).Assembly);
    }
}
