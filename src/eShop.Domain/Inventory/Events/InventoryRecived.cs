namespace eShop.Domain.Inventory.Events;

using eShop.Domain.SharedKernel.Abstractions;
using eShop.Domain.SharedKernel.ValueObjects;

public sealed record InventoryRecived(InventoryItemId InventoryItemId, Quantity Delta)
    : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
