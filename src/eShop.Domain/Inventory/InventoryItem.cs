namespace eShop.Domain.Inventory;

using eShop.Domain.SharedKernel.ValueObjects;
using eShop.Domain.SharedKernel.Abstractions;

public sealed class InventoryItem : AggregateRoot
{
    private InventoryItem(
        InventoryItemId id,
        Sku sku,
        Quantity quantityOnHand,
        Quantity reservedQuantity
    )
    {
        Id = id;
        Sku = sku;
        QuantityOnHand = quantityOnHand;
        ReservedQuantity = reservedQuantity;
    }

    public InventoryItemId Id { get; }
    public Sku Sku { get; private set; }
    public Quantity QuantityOnHand { get; private set; }
    public Quantity ReservedQuantity { get; private set; }

    public Quantity AvailableQuantity => QuantityOnHand - ReservedQuantity;

    public static InventoryItem Create(
        InventoryItemId id,
        Sku sku,
        Quantity quantityOnHand,
        Quantity reservedQuantity
    ) => new InventoryItem(id, sku, quantityOnHand, reservedQuantity);

    public void SetSku(Sku sku) => Sku = sku;

    public void ReceiveStock(Quantity quantity)
    {
        QuantityOnHand += quantity;
    }

    public void ReserveStock(Quantity quantity)
    {
        if (AvailableQuantity < quantity)
            throw new InvalidOperationException(
                $"Insufficient stock for SKU {Sku.Value}. Requested: {quantity}, Available: {AvailableQuantity}"
            );

        ReservedQuantity += quantity;
    }

    public void ConfirmShipment(Quantity quantity)
    {
        if (ReservedQuantity < quantity)
            throw new InvalidOperationException("Cannot ship more than what is reserved.");

        if (QuantityOnHand < quantity)
            throw new InvalidOperationException(
                "Physical stock discrepancy: cannot ship more than on hand."
            );

        ReservedQuantity -= quantity;
        QuantityOnHand -= quantity;
    }

    public void CancelReservation(Quantity quantity)
    {
        if (ReservedQuantity < quantity)
            throw new InvalidOperationException("Cannot cancel more reservations than exist.");

        ReservedQuantity -= quantity;
    }
}
