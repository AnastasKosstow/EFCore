using EFCore.Inheritance.TablePerType.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.Inheritance.TablePerType.EntityTypeConfigurations;

internal class BlogEntityTypeConfiguration : IEntityTypeConfiguration<BillingDetail>
{
    public void Configure(EntityTypeBuilder<BillingDetail> builder)
    {
        builder
            .HasKey(billingDetail => billingDetail.Id);
    }
}
