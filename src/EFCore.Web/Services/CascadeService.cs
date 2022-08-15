using EFCore.Inheritance.Cascade;
using EFCore.Inheritance.Cascade.Models;
using EFCore.Web.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Web.Services;

public class CascadeService : ICascadeService
{
    private readonly CascadeDbContext _context;

    public CascadeService(CascadeDbContext context)
    {
        _context = context;
    }

    public async Task<IResponse> GetResult(CancellationToken cancellationToken)
    {
        if (!_context.Blogs.Any())
        {
            AddInitialData();
        }

        var blog = await _context.Blogs.FirstAsync(cancellationToken);
        var response = new CascadeResponse(_context.Blogs.Remove(blog));

        await _context.SaveChangesAsync(cancellationToken);

        return response;
    }

    #region InitialData
    private void AddInitialData()
    {
        Blog blogToAdd = new()
        {
            Name = "Blog name",
            Posts = new List<Post>()
            {
                new Post() 
                {
                    Title = "Post title 1",
                    Content = "Post content 1"
                },
                new Post()
                {
                    Title = "Post title 2",
                    Content = "Post content 2"
                }
            }
        };

        _context.Blogs.Add(blogToAdd);
        _context.SaveChanges();
    }
    #endregion
}
