using eShop.Domain.Inventory;
using eShop.Domain.SharedKernel.ValueObjects;

namespace eShop.Domain.Tests.Inventory;

public class InventoryItemTests
{
    private readonly InventoryItemId _id = new(Guid.NewGuid());
    private readonly Sku _sku = Sku.Create("TEST-SKU");
    private readonly Quantity _quantityOnHand = Quantity.Create(10);
    private readonly Quantity _reservedQuantity = Quantity.Create(2);

    [Fact]
    public void Create_WithValidParameters_InitializesCorrectly()
    {
        var item = InventoryItem.Create(_id, _sku, _quantityOnHand, _reservedQuantity);

        Assert.Equal(_id, item.Id);
        Assert.Equal(_sku, item.Sku);
        Assert.Equal(_quantityOnHand, item.QuantityOnHand);
        Assert.Equal(_reservedQuantity, item.ReservedQuantity);
        Assert.Equal(Quantity.Create(8), item.AvailableQuantity);
    }

    [Fact]
    public void SetSku_UpdatesSku()
    {
        var item = InventoryItem.Create(_id, _sku, _quantityOnHand, _reservedQuantity);
        var newSku = Sku.Create("NEW-SKU");

        item.SetSku(newSku);

        Assert.Equal(newSku, item.Sku);
    }

    [Fact]
    public void ReceiveStock_IncreasesQuantityOnHand()
    {
        var item = InventoryItem.Create(_id, _sku, _quantityOnHand, _reservedQuantity);
        var quantityToReceive = Quantity.Create(5);

        item.ReceiveStock(quantityToReceive);

        Assert.Equal(Quantity.Create(15), item.QuantityOnHand);
        Assert.Equal(Quantity.Create(13), item.AvailableQuantity);
    }

    [Fact]
    public void ReserveStock_WithSufficientAvailableQuantity_IncreasesReservedQuantity()
    {
        var item = InventoryItem.Create(_id, _sku, _quantityOnHand, _reservedQuantity);
        var quantityToReserve = Quantity.Create(3);

        item.ReserveStock(quantityToReserve);

        Assert.Equal(Quantity.Create(5), item.ReservedQuantity);
        Assert.Equal(Quantity.Create(5), item.AvailableQuantity);
    }

    [Fact]
    public void ReserveStock_WithInsufficientAvailableQuantity_ThrowsInvalidOperationException()
    {
        var item = InventoryItem.Create(_id, _sku, _quantityOnHand, _reservedQuantity);
        var quantityToReserve = Quantity.Create(9);

        var ex = Assert.Throws<InvalidOperationException>(() =>
            item.ReserveStock(quantityToReserve)
        );
        Assert.Contains("Insufficient stock", ex.Message);
    }

    [Fact]
    public void ConfirmShipment_WithSufficientReservedAndOnHandQuantity_DecreasesQuantities()
    {
        var item = InventoryItem.Create(_id, _sku, _quantityOnHand, _reservedQuantity);
        var quantityToShip = Quantity.Create(1);

        item.ConfirmShipment(quantityToShip);

        Assert.Equal(Quantity.Create(9), item.QuantityOnHand);
        Assert.Equal(Quantity.Create(1), item.ReservedQuantity);
        Assert.Equal(Quantity.Create(8), item.AvailableQuantity);
    }

    [Fact]
    public void ConfirmShipment_WithInsufficientReservedQuantity_ThrowsInvalidOperationException()
    {
        var item = InventoryItem.Create(_id, _sku, _quantityOnHand, _reservedQuantity);
        var quantityToShip = Quantity.Create(3);

        var ex = Assert.Throws<InvalidOperationException>(() =>
            item.ConfirmShipment(quantityToShip)
        );
        Assert.Equal("Cannot ship more than what is reserved.", ex.Message);
    }

    [Fact]
    public void ConfirmShipment_WithInsufficientOnHandQuantity_ThrowsInvalidOperationException()
    {
        var item = InventoryItem.Create(_id, _sku, Quantity.Create(1), Quantity.Create(2));
        var quantityToShip = Quantity.Create(2);

        var ex = Assert.Throws<InvalidOperationException>(() =>
            item.ConfirmShipment(quantityToShip)
        );
        Assert.Equal("Physical stock discrepancy: cannot ship more than on hand.", ex.Message);
    }

    [Fact]
    public void CancelReservation_WithSufficientReservedQuantity_DecreasesReservedQuantity()
    {
        var item = InventoryItem.Create(_id, _sku, _quantityOnHand, _reservedQuantity);
        var quantityToCancel = Quantity.Create(1);

        item.CancelReservation(quantityToCancel);

        Assert.Equal(Quantity.Create(1), item.ReservedQuantity);
        Assert.Equal(Quantity.Create(9), item.AvailableQuantity);
    }

    [Fact]
    public void CancelReservation_WithInsufficientReservedQuantity_ThrowsInvalidOperationException()
    {
        var item = InventoryItem.Create(_id, _sku, _quantityOnHand, _reservedQuantity);
        var quantityToCancel = Quantity.Create(3);

        var ex = Assert.Throws<InvalidOperationException>(() =>
            item.CancelReservation(quantityToCancel)
        );
        Assert.Equal("Cannot cancel more reservations than exist.", ex.Message);
    }
}
