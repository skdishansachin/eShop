namespace eShop.Domain.Inventory.Events;

using System;
using eShop.Domain.SharedKernel.Abstractions;
using eShop.Domain.SharedKernel.ValueObjects;

public sealed record InventoryItemCreated(InventoryItemId InventoryItemId, Sku Sku) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
