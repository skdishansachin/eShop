using eShop.Domain.SharedKernel.ValueObjects;

namespace eShop.Domain.Tests.Inventory;

public class InventoryItemIdTests
{
    [Fact]
    public void InventoryItemId_Create_WithValidGuid_ReturnsInventoryItemIdObject()
    {
        var guid = Guid.NewGuid();
        var inventoryItemId = new InventoryItemId(guid);
        Assert.Equal(guid, inventoryItemId.Value);
    }

    [Fact]
    public void InventoryItemId_Equality_WithSameValue_ReturnsTrue()
    {
        var guid = Guid.NewGuid();
        var id1 = new InventoryItemId(guid);
        var id2 = new InventoryItemId(guid);

        Assert.True(id1.Equals(id2));
        Assert.True(id1 == id2);
        Assert.Equal(id1.GetHashCode(), id2.GetHashCode());
    }

    [Fact]
    public void InventoryItemId_Inequality_WithDifferentValue_ReturnsFalse()
    {
        var id1 = new InventoryItemId(Guid.NewGuid());
        var id2 = new InventoryItemId(Guid.NewGuid());

        Assert.False(id1.Equals(id2));
        Assert.True(id1 != id2);
    }
}
