<h3 align="center">
EF Core samples
</h3>

****

Table of content
-----------------

* [Inheritance](#inheritance)

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

 - Table per Type is about representing inheritance relationships as relational foreign key associations.
 - Every class and subclass including abstract classes has its own table.
 - The table for subclasses contains columns only for each noninherited property along with a primary key that is also a foreign key of the base class table.

Models

Each user has a navigation property: BillingInfo
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
