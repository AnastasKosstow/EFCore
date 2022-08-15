namespace EFCore.Inheritance.Cascade.Models;

public class Blog
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IList<Post> Posts { get; set; } = new List<Post>();
}
