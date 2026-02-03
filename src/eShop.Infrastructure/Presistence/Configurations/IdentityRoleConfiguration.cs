using eShop.Domain.SharedKernel.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Infrastructure.Presistence.Configurations;

public sealed class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole<UserId>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<UserId>> builder)
    {
        builder.ToTable("AspNetRoles", "Identity");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id).HasConversion(id => id.Value, value => new UserId(value));
    }
}
