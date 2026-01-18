namespace eShop.Domain.Orders;

using eShop.Domain.SharedKernel.Abstractions;
using eShop.Domain.SharedKernel.ValueObjects;

public sealed class Order : AggregateRoot
{
    public Order(OrderId id, CustomerId customerId)
    {
        Id = id;
        CustomerId = customerId;
        CreatedAt = DateTimeOffset.UtcNow;
        TotalPrice = Money.Create(0, "LKR");
        Status = OrderStatus.Pending;
    }

    public OrderId Id { get; }
    public CustomerId CustomerId { get; }
    public DateTimeOffset CreatedAt { get; }
    public Money TotalPrice { get; private set; }
    public OrderStatus Status { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public static Order Create(OrderId id, CustomerId customerId) => new Order(id, customerId);

    public void AddItem(Sku sku, Money unitPrice, Quantity quantity)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException(
                "Cannot modify an order that is no longer pending."
            );

        var item = _items.FirstOrDefault(i => i.Sku == sku);
        if (item is not null) // Item already exists
            throw new InvalidOperationException(
                "Item already exists in order. Update quantity instead."
            );

        // Item does not exist, so add it
        var orderItemId = new OrderItemId(Guid.NewGuid());
        _items.Add(new OrderItem(orderItemId, sku, quantity, unitPrice));

        RecalculateTotal();
    }

    public void Ship()
    {
        EnsureStatus(OrderStatus.Confirmed);
        Status = OrderStatus.Shipped;
    }

    public void Confirm()
    {
        if (!_items.Any())
            throw new InvalidOperationException("Cannot confirm an empty order.");

        Status = OrderStatus.Confirmed;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Shipped)
            throw new InvalidOperationException("Cannot cancel an order that has already shipped.");

        Status = OrderStatus.Cancelled;
    }

    private void RecalculateTotal()
    {
        if (!_items.Any())
        {
            TotalPrice = Money.Create(0, TotalPrice.Currency);
            return;
        }

        var currency = _items.First().PriceAtPurchase.Currency;
        var totalAmount = _items.Sum(item => item.PriceAtPurchase.Amount);

        TotalPrice = Money.Create(totalAmount, currency);
    }

    private void EnsureStatus(OrderStatus status)
    {
        if (Status != status)
            throw new InvalidOperationException(
                $"Action not allowed. Order is in {Status} state, but must be {status}."
            );
    }
}
