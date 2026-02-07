using eShop.Domain.SharedKernel.ValueObjects;

namespace eShop.Domain.Inventory;

public interface IInventoryItemRepository
{
    Task<InventoryItem?> GetByIdAsync(InventoryItemId id);
    Task<InventoryItem?> GetBySkuAsync(Sku sku);
}
