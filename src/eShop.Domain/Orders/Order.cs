namespace eShop.Domain.Orders;

using eShop.Domain.Orders.Events;
using eShop.Domain.SharedKernel.Abstractions;
using eShop.Domain.SharedKernel.ValueObjects;

public sealed class Order : AggregateRoot
{
    public Order(OrderId id, CustomerId customerId)
    {
        Id = id;
        CustomerId = customerId;
        CreatedAt = DateTimeOffset.UtcNow;
        TotalPrice = Money.Create(0, "LKR"); // TODO: Fix hardcoded currency
        Status = OrderStatus.Pending;
    }

    public OrderId Id { get; }
    public CustomerId CustomerId { get; }
    public DateTimeOffset CreatedAt { get; }
    public Money TotalPrice { get; private set; }
    public OrderStatus Status { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public static Order Create(OrderId id, CustomerId customerId)
    {
        var order = new Order(id, customerId);
        order.RaiseEvent(new OrderCreated(id, customerId));

        return order;
    }

    public void AddItem(Sku sku, Money unitPrice, Quantity quantity)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException(
                "Cannot modify an order that is no longer pending."
            );

        if (unitPrice.Currency != TotalPrice.Currency)
            throw new InvalidOperationException("Currency mismatch.");

        var item = _items.FirstOrDefault(i => i.Sku == sku);
        if (item is not null) // Item already exists
            throw new InvalidOperationException(
                "Item already exists in order. Update quantity instead."
            );

        _items.Add(new OrderItem(new OrderItemId(Guid.NewGuid()), sku, quantity, unitPrice));

        RecalculateTotal();
    }

    public void Ship()
    {
        EnsureStatus(OrderStatus.Paid);
        Status = OrderStatus.Shipped;
    }

    public void Paid()
    {
        EnsureStatus(OrderStatus.Pending);
        if (!_items.Any())
            throw new InvalidOperationException("Cannot paid an empty order.");

        Status = OrderStatus.Paid;

        RaiseEvent(new OrderPaid(Id, TotalPrice));
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Shipped)
            throw new InvalidOperationException("Cannot cancel an order that has already shipped.");

        Status = OrderStatus.Cancelled;

        RaiseEvent(new OrderCancelled(Id));
    }

    private void RecalculateTotal()
    {
        if (!_items.Any())
        {
            TotalPrice = Money.Create(0, TotalPrice.Currency);
            return;
        }

        var currency = _items.First().PriceAtPurchase.Currency;
        var totalAmount = _items.Sum(item => item.TotalPrice.Amount);

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
