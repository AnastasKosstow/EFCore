using EFCore.Indexes;
using EFCore.Web.DatabaseConfigurator;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// To add migration and create database:
// Run commands:
// -> Add-Migration {migration name} -Context {context name}
// -> Update-Database -Context {context name}

builder.Services.AddDatabase(config =>
{
    config.UsePostgreSQL<IndexesDbContext>();
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/", async (HttpContext context) =>
{
    // Get specific context
    var dbContext = context.RequestServices.GetService<IndexesDbContext>();

    // Try it...

    // Example of 'IndexesDbContext' and the use of GIN (Generalized Inverted Index)
    var searchQuery = "Blog description - 415";
    var searchResult = await dbContext.Blogs
        .Where(e => e.Search.Matches(EF.Functions.PhraseToTsQuery("simple", searchQuery)))
    .FirstOrDefaultAsync();


    // If dbContext is null, you need to add it first in 'AddDatabase' configuration
});

app.Run();
