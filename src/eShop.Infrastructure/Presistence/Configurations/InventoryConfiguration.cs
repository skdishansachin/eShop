using eShop.Domain.Inventory;
using eShop.Domain.SharedKernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Infrastructure.Presistence.Configurations;

public sealed class InventoryConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("InventoryItem", "Inventory");

        builder.HasKey(i => i.Id);
        builder
            .Property(i => i.Id)
            .HasConversion(id => id.Value, value => new InventoryItemId(value));

        builder
            .Property(i => i.Sku)
            .HasConversion(sku => sku.Value, value => Sku.Create(value))
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(i => i.QuantityOnHand)
            .HasConversion(quantity => quantity.Value, value => Quantity.Create(value))
            .IsRequired();

        builder
            .Property(i => i.ReservedQuantity)
            .HasConversion(quantity => quantity.Value, value => Quantity.Create(value))
            .IsRequired();
    }
}
