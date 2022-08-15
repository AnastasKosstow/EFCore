namespace EFCore.Inheritance.TablePerHierarchy.Models;

public class EFBlog : Blog
{
    public override string GetBlogType()
        => GetType().Name;
}
