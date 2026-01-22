namespace eShop.Domain.Catalog.Events;

using eShop.Domain.SharedKernel.Abstractions;
using eShop.Domain.SharedKernel.ValueObjects;

public sealed record ProductVariantAdded(
    ProductId ProductId,
    ProductVariantId ProductVariantId,
    Sku Sku
) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
