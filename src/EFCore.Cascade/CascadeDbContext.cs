using EFCore.Cascade.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Cascade;

public class CascadeDbContext : DbContext
{
    public CascadeDbContext(DbContextOptions<CascadeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Blog> Blogs { get; set; } = default;
    public DbSet<Post> Posts { get; set; } = default;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CascadeDbContext).Assembly);
    }
}
