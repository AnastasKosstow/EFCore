using EFCore.Inheritance.Cascade;
using EFCore.Inheritance.TablePerHierarchy;
using EFCore.Inheritance.TablePerType;
using EFCore.Web.Extensions;
using EFCore.Web.Services;
using EFCore.Web.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// To add migration and create database:
// Run for each context:
// -> Add-Migration {migration name} -Context {context name}
// -> Update-Database -Context {context name}

builder.Services.AddContext<TablePerTypeDbContext>(
    builder.Configuration.GetConnectionString("TPT_Connection"));

builder.Services.AddContext<TablePerHierarchyDbContext>(
    builder.Configuration.GetConnectionString("TPH_Connection"));

builder.Services.AddContext<CascadeDbContext>(
    builder.Configuration.GetConnectionString("Cascade_Connection"));

builder.Services
    .AddScoped<ICascadeService, CascadeService>()
    .AddScoped<ITablePerTypeService, TablePerTypeService>()
    .AddScoped<ITablePerHierarchyService, TablePerHierarchyService>();

var app = builder.Build();

app.UseHttpsRedirection();

#region ENDPOINTS
app.MapGet("/perType", async (ITablePerTypeService service, CancellationToken cancellationToken) =>
{
    return await service.GetResult(cancellationToken);
});

app.MapGet("/perHierarchy", async (ITablePerHierarchyService service, CancellationToken cancellationToken) =>
{
    return await service.GetResult(cancellationToken);
});

/// <summary>
/// For Cascade - See EntityTypeConfigurations of Cascade project.
/// </summary>
app.MapGet("/cascade", async (ICascadeService service, CancellationToken cancellationToken) =>
{
    return await service.GetResult(cancellationToken);
});
#endregion

app.Run();
