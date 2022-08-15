using EFCore.Inheritance.TablePerHierarchy;
using EFCore.Inheritance.TablePerHierarchy.Models;
using EFCore.Web.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Web.Services;

public class TablePerHierarchyService : ITablePerHierarchyService
{
    private readonly TablePerHierarchyDbContext _context;

    public TablePerHierarchyService(TablePerHierarchyDbContext context)
    {
        _context = context;
    }

    public async Task<IResponse> GetResult(CancellationToken cancellationToken)
    {
        if (!_context.Blogs.Any())
        {
            AddInitialData();
        }

        // To get all by Type: _context.Blogs.OfType<EFBlog>()

        var blog = await _context
            .Blogs?
            .FirstOrDefaultAsync(cancellationToken);

        return new TablePerHierarchyResponse(blog.Discriminator);
    }

    #region InitialData
    private void AddInitialData()
    {
        Blog blogToAdd = new EFBlog
        {
            Discriminator = BlogType.EF
        };

        _context.Blogs.Add(blogToAdd);
        _context.SaveChanges();
    }
    #endregion
}
