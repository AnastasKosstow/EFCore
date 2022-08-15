using EFCore.Inheritance.TablePerHierarchy.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Inheritance.TablePerHierarchy;

public class TablePerHierarchyDbContext : DbContext
{
    public TablePerHierarchyDbContext(DbContextOptions<TablePerHierarchyDbContext> options)
        : base(options)
    {
    }

    public DbSet<Blog> Blogs { get; set; } = default;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TablePerHierarchyDbContext).Assembly);
    }
}
