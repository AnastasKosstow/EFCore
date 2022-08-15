namespace EFCore.Inheritance.TablePerHierarchy.Models;

public class RssBlog : Blog
{
    public override string GetBlogType()
        => GetType().Name;
}