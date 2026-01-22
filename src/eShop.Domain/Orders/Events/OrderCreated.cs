namespace eShop.Domain.Orders.Events;

using eShop.Domain.SharedKernel.Abstractions;
using eShop.Domain.SharedKernel.ValueObjects;

public sealed record OrderPlaced(OrderId OrderId, CustomerId CustomerId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
