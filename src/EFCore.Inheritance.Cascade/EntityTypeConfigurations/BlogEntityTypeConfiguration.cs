using EFCore.Inheritance.Cascade.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Inheritance.Cascade.EntityTypeConfigurations;

internal class BlogEntityTypeConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder
            .HasKey(blog => blog.Id);

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
    }
}
