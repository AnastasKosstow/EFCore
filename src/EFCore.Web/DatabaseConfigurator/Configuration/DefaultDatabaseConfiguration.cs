using EFCore.Indexes;
using EFCore.Web.DatabaseConfigurator.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EFCore.Web.DatabaseConfigurator.Configuration;

public class DefaultDatabaseConfiguration : IDatabaseConfiguration
{
    public IServiceCollection _services { get; }

    public DefaultDatabaseConfiguration(IServiceCollection services)
    {
        _services = services;
    }

    public IServiceCollection UseSQLServer<T>()  where T : DbContext
    {
        var connectionOptions = GetConnectionOptions();
        _services.AddDbContext<T>(options =>
            options.UseSqlServer(connectionOptions.ConnectionString));

        return _services;
    }

    public IServiceCollection UsePostgreSQL<T>() where T : DbContext
    {
        var connectionOptions = GetConnectionOptions();
        _services.AddDbContext<T>(options => 
            options.UseNpgsql(connectionOptions.ConnectionString));

        return _services;
    }

    private DatabaseConnectionOptions GetConnectionOptions()
    {
        IOptions<DatabaseConnectionOptions> connectionOptions;
        using (var serviceProvider = _services.BuildServiceProvider())
        {
            connectionOptions = serviceProvider.GetService<IOptions<DatabaseConnectionOptions>>();
        }

        return connectionOptions.Value;
    }
}
