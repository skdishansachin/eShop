namespace eShop.Domain.Catalog.Events;

using eShop.Domain.SharedKernel.Abstractions;
using eShop.Domain.SharedKernel.ValueObjects;

public sealed record ProductPriceChanged(ProductId ProductId, Money OldPrice, Money NewPrice)
    : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
