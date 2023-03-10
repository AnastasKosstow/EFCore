using EFCore.Web.DatabaseConfigurator.Configuration;
using EFCore.Web.DatabaseConfigurator.Options;

namespace EFCore.Web.DatabaseConfigurator;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(
            this IServiceCollection services,
            Action<IDatabaseConfiguration> configAction)
    {
        if (configAction is null)
        {
            throw new ArgumentNullException($"{nameof(configAction)} is null. Configuration cannot be null or empty!");
        }

        services.AddOptions<DatabaseConnectionOptions>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("DbConnection").Bind(settings);
            });

        var config = new DefaultDatabaseConfiguration(services);
        configAction?.Invoke(config);
        return services;
    }
}
