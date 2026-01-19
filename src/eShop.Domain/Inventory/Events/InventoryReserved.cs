namespace eShop.Domain.Inventory.Events;

using eShop.Domain.SharedKernel.Abstractions;
using eShop.Domain.SharedKernel.ValueObjects;

public sealed record InventoryReserved(InventoryItemId InventoryItemId, OrderId OrderId, Quantity Quantity) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
