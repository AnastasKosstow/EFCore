using Microsoft.EntityFrameworkCore;

namespace EFCore.Web.DatabaseConfigurator.Configuration;

public interface IDatabaseConfiguration
{
    IServiceCollection UseSQLServer<T>() where T : DbContext;
    IServiceCollection UsePostgreSQL<T>() where T : DbContext;
}
