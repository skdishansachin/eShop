namespace eShop.Domain.Orders.Events;

using eShop.Domain.SharedKernel.Abstractions;
using eShop.Domain.SharedKernel.ValueObjects;

public sealed record OrderCancelled(OrderId OrderId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
