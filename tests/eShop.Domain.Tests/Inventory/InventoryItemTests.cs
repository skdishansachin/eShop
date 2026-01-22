using eShop.Domain.Inventory;
using eShop.Domain.Inventory.Events;
using eShop.Domain.SharedKernel.ValueObjects;

namespace eShop.Domain.Tests.Inventory;

public class InventoryItemTests
{
    [Fact]
    public void Create_WithValidParameters_InitializesCorrectly()
    {
        var id = new InventoryItemId(Guid.NewGuid());
        var sku = Sku.Create("TEST-SKU");
        var onHand = Quantity.Create(10);
        var reserved = Quantity.Create(2);

        var item = InventoryItem.Create(id, sku, onHand, reserved);

        Assert.Equal(id, item.Id);
        Assert.Equal(sku, item.Sku);
        Assert.Equal(onHand, item.QuantityOnHand);
        Assert.Equal(reserved, item.ReservedQuantity);
        Assert.Equal(Quantity.Create(8), item.AvailableQuantity);

        Assert.Single(item.DomainEvents);
        Assert.Contains(new InventoryItemCreated(item.Id, sku), item.DomainEvents);
    }

    [Fact]
    public void ReceiveStock_ShouldIncreasePhysicalAndAvailableStock()
    {
        var item = CreateItem(onHand: 10, reserved: 2);
        var quantityToReceive = Quantity.Create(5);

        item.ReceiveStock(quantityToReceive);

        Assert.Equal(Quantity.Create(15), item.QuantityOnHand);
        Assert.Equal(Quantity.Create(13), item.AvailableQuantity);
        Assert.Contains(new InventoryRecived(item.Id, quantityToReceive), item.DomainEvents);
    }

    [Fact]
    public void ReserveStock_WithSufficientStock_ShouldIncreaseReservedQuantity()
    {
        var orderId = new OrderId(Guid.NewGuid());
        var item = CreateItem(onHand: 10, reserved: 2);
        var quantityToReserve = Quantity.Create(3);

        item.ReserveStock(orderId, quantityToReserve);

        Assert.Equal(Quantity.Create(5), item.ReservedQuantity);
        Assert.Equal(Quantity.Create(5), item.AvailableQuantity);

        Assert.Contains(
            new InventoryReserved(item.Id, orderId, quantityToReserve),
            item.DomainEvents
        );
    }

    [Fact]
    public void ReserveStock_ShouldRaiseThresholdEvent_WhenStockDropsLow()
    {
        var orderId = new OrderId(Guid.NewGuid());
        var item = CreateItem(onHand: 10, reserved: 0);
        var quantityToReserve = Quantity.Create(6);

        item.ReserveStock(orderId, quantityToReserve);

        Assert.Equal(Quantity.Create(4), item.AvailableQuantity);
        Assert.Contains(
            new InventoryReserved(item.Id, orderId, quantityToReserve),
            item.DomainEvents
        );
    }

    [Fact]
    public void ReserveStock_WithInsufficientStock_ShouldThrowException()
    {
        var orderId = new OrderId(Guid.NewGuid());
        var item = CreateItem(onHand: 10, reserved: 2);

        var ex = Assert.Throws<InvalidOperationException>(() =>
            item.ReserveStock(orderId, Quantity.Create(9))
        );
        Assert.Contains("Insufficient stock", ex.Message);
    }

    [Fact]
    public void ConfirmShipment_ShouldDecreaseBothQuantities()
    {
        var item = CreateItem(onHand: 10, reserved: 5);
        var quantityToShip = Quantity.Create(3);

        item.ConfirmShipment(quantityToShip);

        Assert.Equal(Quantity.Create(7), item.QuantityOnHand);
        Assert.Equal(Quantity.Create(2), item.ReservedQuantity);
    }

    [Fact]
    public void CancelReservation_ShouldRestoreAvailableQuantity()
    {
        var item = CreateItem(onHand: 10, reserved: 5);

        item.CancelReservation(Quantity.Create(2));

        Assert.Equal(Quantity.Create(3), item.ReservedQuantity);
        Assert.Equal(Quantity.Create(7), item.AvailableQuantity);
    }

    private static InventoryItem CreateItem(
        int onHand = 10,
        int reserved = 0,
        string skuCode = "TEST-SKU"
    )
    {
        return InventoryItem.Create(
            new InventoryItemId(Guid.NewGuid()),
            Sku.Create(skuCode),
            Quantity.Create(onHand),
            Quantity.Create(reserved)
        );
    }
}
