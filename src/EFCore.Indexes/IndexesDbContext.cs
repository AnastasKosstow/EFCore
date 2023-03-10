using EFCore.Indexes.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Indexes;

public class IndexesDbContext : DbContext
{
    public IndexesDbContext(DbContextOptions<IndexesDbContext> options)
        : base(options)
    {
    }

    public DbSet<Blog> Blogs { get; set; } = default;
    public DbSet<Post> Posts { get; set; } = default;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IndexesDbContext).Assembly);
    }
}
