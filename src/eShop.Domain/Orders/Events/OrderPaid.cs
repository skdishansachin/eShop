namespace eShop.Domain.Orders.Events;

using eShop.Domain.SharedKernel.Abstractions;
using eShop.Domain.SharedKernel.ValueObjects;

public sealed record OrderPaid(OrderId OrderId, Money Amount) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
