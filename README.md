<h3 align="center">
EF Core samples
</h3>

****

Table of content
-----------------

* [Inheritance](#inheritance)
* [Cascade delete](#cascade-delete)
* [Indexes](#indexes)
 

Inheritance
==========================

In plain words
> EF can map a .NET type hierarchy to a database. This allows you to write your .NET entities in code as usual, using base and derived types, and have EF seamlessly create the appropriate database schema, issue queries, etc. 

* [Table per hierarchy](#table-per-hierarchy)
* [Table per type](#table-per-type)

Table per hierarchy
--------------
> By default, EF maps the inheritance using the table-per-hierarchy (TPH) pattern. TPH uses a single table to store the data for all types in the hierarchy, and a discriminator column is used to identify which type each row represents.

Models
```C#
public enum BlogType : short
{
    EF = 1,
    RSS = 2
}

public abstract class Blog
{
    public int Id { get; set; }

    public BlogType Discriminator { get; set; }
}

public class EFBlog : Blog
{
    public string TypeName { get; } = BlogType.EF.ToString();
}

public class RssBlog : Blog
{
    public string TypeName { get; } = BlogType.RSS.ToString();
}
```

> You can configure the name and type of the discriminator column and the values that are used to identify each type in the hierarchy:
```C#
internal class BlogEntityTypeConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder
            .HasDiscriminator(policy => policy.Discriminator)
            .HasValue<EFBlog>(BlogType.EF)
            .HasValue<RssBlog>(BlogType.RSS);
    }
}
```

Table per type
--------------
> Table-per-type inheritance uses a separate table in the database to maintain data for non-inherited properties and key properties for each type in the inheritance hierarchy.

> Table per Type is about representing inheritance relationships as relational foreign key associations. Every class and subclass including abstract classes has its own table. The table for subclasses contains columns only for each noninherited property along with a primary key that is also a foreign key of the base class table.

> Each user has a navigation property: BillingInfo
```C#
public class User
{
    public int Id { get; set; }
    ...

    public virtual BillingDetail BillingInfo { get; set; }
}
```

```C#
public abstract record BillingDetail
{
    public int Id { get; set; }
    public string Owner { get; set; }
    public string Number { get; set; }
}

public record BankAccount : BillingDetail
{
    public string BankName { get; set; }
}

public record CreditCard : BillingDetail
{
    public string CardType { get; set; }
}
```

> And the configuration: 
```C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<BankAccount>().ToTable("BankAccounts");
    modelBuilder.Entity<CreditCard>().ToTable("CreditCards");

    modelBuilder.ApplyConfigurationsFromAssembly(typeof(TablePerTypeDbContext).Assembly);
}
```

<strong>The base class and subclasses have its own table. 
The table for subclasses contains columns only for each noninherited property along with a primary key that is also a foreign key of the base class table.</strong>


Cascade delete
==========================

<strong>Cascade delete allows the deletion of a row to trigger the deletion of related rows automatically.</strong>

> EF Core covers a closely related concept and implements several different delete behaviors and allows for the configuration of the delete behaviors of individual relationships.
> In Entity Framework Core, the OnDelete Fluent API method is used to specify the delete behavior for a dependent entity when the principal is deleted.
> The OnDelete method takes a DeleteBehavior enum as a parameter:

<strong>Cascade:</strong> Child/dependent entity should be deleted
<strong>Restrict:</strong> Dependents are unaffected
<strong>SetNull:</strong> The foreign key values in dependent rows should update to NULL 

To configure Cascade delete use EntityTypeConfiguration:
```C#
    // DeleteBehavior.Cascade
    // Entities will be deleted when the related principal is deleted
    builder
        .HasMany(x => x.Posts)
        .WithOne(x => x.Blog)
        .OnDelete(DeleteBehavior.Cascade);

    // DeleteBehavior.Restrict
    // Values of foreign key properties in dependent entities are set to null when the related principal is deleted
    // In order to work, Blog navigation property in Post must be nullable
    builder
        .HasMany(x => x.Posts)
        .WithOne(x => x.Blog)
        .OnDelete(DeleteBehavior.Restrict);
```

<h4>Delete Behaviors</h4>

Optional Relationships (nullable foreign key)

| Behavior Name   | Effect on dependent/child in memory | Effect on dependent/child in database |
|-----------------|-------------------------------------|---------------------------------------|
| Cascade | Entities are deleted | Entities are deleted
| ClientSetNull (Default) | Foreign key properties are set to null | None
| SetNull | Foreign key properties are set to null | Foreign key properties are set to null
| Restrict | None | None

Required Relationships (non-nullable foreign key)

| Behavior Name   | Effect on dependent/child in memory | Effect on dependent/child in database |
|-----------------|-------------------------------------|---------------------------------------|
| Cascade | Entities are deleted | Entities are deleted
| ClientSetNull (Default) | SaveChanges throws | None
| SetNull | SaveChanges throws | SaveChanges throws
| Restrict | None | None



Indexes
==========================

<strong>In SQL, indexes are data structures that improve the performance of database queries. An index is a data structure that is built on one or more columns of a table. The index stores a copy of the indexed column(s) in a separate data structure, along with a reference to the original row in the table.

When a query is executed against a table, the database engine can use the index to quickly locate the rows that match the query criteria. This can significantly improve the performance of queries, especially when the table contains a large number of rows.</strong>

<strong>Pros:</strong>
> Faster query performance: Indexes can significantly improve the performance of queries by allowing the database engine to quickly locate the rows that match the query criteria.
> Sorting: Indexes can also improve the performance of sorting operations, by allowing the database engine to read the data in a pre-sorted order.
> Clustering: Indexes can be used to cluster related rows of data together, improving query performance for related data.

<strong>Cons:</strong>
> Increased storage: Indexes require additional storage space on disk, which can be a significant consideration for large databases.
> Increased overhead: Indexes also require additional processing time to create and maintain, which can impact overall system performance.
> Insert and update performance: When inserting or updating data in a table with indexes, the indexes must be updated as well, which can slow down the performance of these operations.

B-tree index
```C#
    builder
        .HasIndex(blog => blog.CreatedOn)
        .IsClustered(true);
```

Multi-column index
```C#
    builder
        .HasIndex(blog => blog.CreatedOn)
        .IsClustered(true);
```

Generalized Inverted Index
```C#
    builder
        .HasGeneratedTsVectorColumn(
             blog => blog.Search,
             config: "simple",
             blog => new { blog.Name, blog.Description })
        .HasIndex(blog => blog.Search)
        .HasMethod("GIN");
```

For more information about different types of indexes see BlogEntityTypeConfiguration class in `EFCore.Indexes` project.
