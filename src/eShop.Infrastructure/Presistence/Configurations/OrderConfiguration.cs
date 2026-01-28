using eShop.Domain.Orders;
using eShop.Domain.SharedKernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eShop.Infrastructure.Presistence.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", "Sales");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasConversion(id => id.Value, value => new OrderId(value));

        builder
            .Property(o => o.CustomerId)
            .HasConversion(id => id.Value, value => new CustomerId(value))
            .IsRequired();

        builder.Property(o => o.CreatedAt).IsRequired();

        builder
            .Property(o => o.TotalPrice)
            .HasConversion(money => money.Amount, value => Money.Create(value, "LKR"))
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(o => o.Status).HasConversion<string>().IsRequired();

        builder.OwnsMany(
            o => o.Items,
            items =>
            {
                items.ToTable("OrderItems", "Sales");

                items.WithOwner().HasForeignKey("OrderId");

                items.HasKey(i => i.Id);
                items
                    .Property(i => i.Id)
                    .HasConversion(id => id.Value, value => new OrderItemId(value));

                items
                    .Property(i => i.Sku)
                    .HasConversion(sku => sku.Value, value => Sku.Create(value))
                    .IsRequired();

                items
                    .Property(i => i.Quantity)
                    .HasConversion(quantity => quantity.Value, value => Quantity.Create(value))
                    .IsRequired();

                items
                    .Property(i => i.PriceAtPurchase)
                    .HasConversion(money => money.Amount, value => Money.Create(value, "USD"))
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
            }
        );
    }
}
