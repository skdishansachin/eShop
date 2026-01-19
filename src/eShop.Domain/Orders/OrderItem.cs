namespace eShop.Domain.Orders;

using eShop.Domain.SharedKernel.ValueObjects;

public sealed class OrderItem
{
    internal OrderItem(OrderItemId id, Sku sku, Quantity quantity, Money unitPrice)
    {
        Id = id;
        Sku = sku;
        Quantity = quantity;
        PriceAtPurchase = unitPrice;
    }

    public OrderItemId Id { get; }
    public Sku Sku { get; }
    public Quantity Quantity { get; }
    public Money PriceAtPurchase { get; }

    public Money TotalPrice => PriceAtPurchase * Quantity.Value;
}
