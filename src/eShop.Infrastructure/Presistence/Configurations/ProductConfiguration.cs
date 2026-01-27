using eShop.Domain.Catalog;
using eShop.Domain.SharedKernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Infrastructure.Presistence.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products", "Catalog");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasConversion(id => id.Value, value => new ProductId(value));

        builder.Property(p => p.Title).HasMaxLength(1000).IsRequired();

        builder.Property(p => p.Description).HasMaxLength(1000).IsRequired();

        builder.OwnsMany(
            p => p.Options,
            options =>
            {
                options.ToTable("ProductOptions", "Catalog");

                options.WithOwner().HasForeignKey("ProductId");

                options.HasKey(o => o.Id);
                options
                    .Property(o => o.Id)
                    .HasConversion(id => id.Value, value => new ProductOptionId(value));

                options
                    .Property(o => o.Name)
                    .HasConversion(name => name.Value, value => OptionName.Create(value))
                    .HasMaxLength(200) // TODO Enfroce the max-length
                    .IsRequired();

                options.OwnsMany(
                    o => o.Values,
                    values =>
                    {
                        values.ToTable("ProductOptionValues", "Catalog");

                        values.WithOwner().HasForeignKey("ProductOptionId");

                        values.HasKey(v => v.Id);
                        values
                            .Property(v => v.Id)
                            .HasConversion(id => id.Value, value => new OptionValueId(value));

                        values
                            .Property(v => v.Name)
                            .HasConversion(
                                name => name.Value,
                                value => OptionValueName.Create(value)
                            )
                            .HasMaxLength(200) // TODO enfroce rules
                            .IsRequired();
                    }
                );
            }
        );

        builder.OwnsMany(
            p => p.Variants,
            variants =>
            {
                variants.ToTable("ProductVariants", "Catalog");

                variants.WithOwner().HasForeignKey("ProductId");

                variants.HasKey(v => v.Id);
                variants
                    .Property(v => v.Id)
                    .HasConversion(id => id.Value, value => new ProductVariantId(value));

                variants
                    .Property(v => v.Sku)
                    .HasConversion(sku => sku.Value, value => Sku.Create(value))
                    .HasMaxLength(50) // TODO enfroce the rule
                    .IsRequired();

                variants
                    .Property(v => v.Price)
                    .HasConversion(money => money.Amount, amount => Money.Create(amount, "USD"))
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                variants.OwnsMany(
                    v => v.Selections,
                    selections =>
                    {
                        selections.ToTable("ProductVariantSelections", "Catalog");

                        selections.WithOwner().HasForeignKey("ProductVariantId");

                        selections.HasKey(s => new { s.ProductOptionId, s.OptionValueId });
                        selections
                            .Property(s => s.ProductOptionId)
                            .HasConversion(id => id.Value, value => new ProductOptionId(value));
                        selections
                            .Property(s => s.OptionValueId)
                            .HasConversion(id => id.Value, value => new OptionValueId(value));
                    }
                );
            }
        );
    }
}
