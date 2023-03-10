using NpgsqlTypes;

namespace EFCore.Indexes.Models;

public class Blog
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedOn { get; set; } // b-tree index
    public NpgsqlTsVector Search { get; set; } // GIN index
    public IList<Post> Posts { get; set; } = new List<Post>();
}
