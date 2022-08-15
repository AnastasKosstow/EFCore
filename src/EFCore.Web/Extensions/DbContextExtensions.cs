using Microsoft.EntityFrameworkCore;

namespace EFCore.Web.Extensions;

public static class DbContextExtensions
{
    public static IServiceCollection AddContext<TContext>(this IServiceCollection services, string ConnectionString)
        where TContext : DbContext
        =>
        services.AddDbContext<TContext>(options =>
            options.UseSqlServer(ConnectionString));
}
