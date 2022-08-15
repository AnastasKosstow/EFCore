using EFCore.Inheritance.TablePerHierarchy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Inheritance.TablePerHierarchy.EntityTypeConfigurations;

internal class BlogEntityTypeConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder
            .HasKey(blog => blog.Id);

        builder
            .HasDiscriminator(policy => policy.Discriminator)
            .HasValue<EFBlog>(BlogType.EF)
            .HasValue<RssBlog>(BlogType.ADO);
    }
}
