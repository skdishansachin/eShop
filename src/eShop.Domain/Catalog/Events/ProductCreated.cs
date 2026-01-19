namespace eShop.Domain.Catalog.Events;

using eShop.Domain.SharedKernel.Abstractions;
using eShop.Domain.SharedKernel.ValueObjects;

public sealed record ProductCreated(ProductId ProductId, string? Title) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
