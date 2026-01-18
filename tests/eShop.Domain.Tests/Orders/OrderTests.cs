using System;
using System.Linq;
using Xunit;
using eShop.Domain.Orders;
using eShop.Domain.SharedKernel.ValueObjects;

namespace eShop.Domain.Tests.Orders;

public class OrderTests
{
    private readonly OrderId _orderId = new OrderId(Guid.NewGuid());
    private readonly CustomerId _customerId = new CustomerId(Guid.NewGuid());
    private readonly Sku _testSku = Sku.Create("TEST-SKU");
    private readonly Money _testPrice = Money.Create(10.0m, "LKR");
    private readonly Quantity _testQuantity = Quantity.Create(2);

    [Fact]
    public void Order_Creation_InitializesCorrectly()
    {
        var order = new Order(_orderId, _customerId);

        Assert.Equal(_orderId, order.Id);
        Assert.Equal(_customerId, order.CustomerId);
        Assert.NotEqual(default, order.CreatedAt);
        Assert.Equal(Money.Create(0, "LKR"), order.TotalPrice);
        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Empty(order.Items);
    }

    [Fact]
    public void Order_Create_FactoryMethodInitializesCorrectly()
    {
        var order = Order.Create(_orderId, _customerId);

        Assert.Equal(_orderId, order.Id);
        Assert.Equal(_customerId, order.CustomerId);
        Assert.NotEqual(default, order.CreatedAt);
        Assert.Equal(Money.Create(0, "LKR"), order.TotalPrice);
        Assert.Equal(OrderStatus.Pending, order.Status);
        Assert.Empty(order.Items);
    }

    [Fact]
    public void Order_AddItem_AddsNewItemAndRecalculatesTotal()
    {
        var order = Order.Create(_orderId, _customerId);

        order.AddItem(_testSku, _testPrice, _testQuantity);

        Assert.Single(order.Items);
        var addedItem = order.Items.First();
        Assert.Equal(_testSku, addedItem.Sku);
        Assert.Equal(_testPrice, addedItem.PriceAtPurchase);
        Assert.Equal(_testQuantity, addedItem.Quantity);
        Assert.Equal(Money.Create(20.0m, "LKR"), order.TotalPrice);
    }

    [Fact]
    public void Order_AddItem_ThrowsException_WhenAddingExistingSku()
    {
        var order = Order.Create(_orderId, _customerId);
        order.AddItem(_testSku, _testPrice, _testQuantity);

        var exception = Assert.Throws<InvalidOperationException>(
            () => order.AddItem(_testSku, _testPrice, _testQuantity)
        );
        Assert.Contains("Item already exists in order. Update quantity instead.", exception.Message);
        Assert.Single(order.Items); // Ensure no new item was added
    }

    [Fact]
    public void Order_AddItem_ThrowsException_WhenOrderNotPending()
    {
        var order = Order.Create(_orderId, _customerId);
        order.AddItem(_testSku, _testPrice, _testQuantity);
        order.Confirm(); // Change status to Confirmed

        var exception = Assert.Throws<InvalidOperationException>(
            () => order.AddItem(Sku.Create("NEW-SKU"), Money.Create(5, "LKR"), Quantity.Create(1))
        );
        Assert.Contains("Cannot modify an order that is no longer pending.", exception.Message);
    }

    [Fact]
    public void Order_Confirm_ChangesStatusToConfirmed()
    {
        var order = Order.Create(_orderId, _customerId);
        order.AddItem(_testSku, _testPrice, _testQuantity);

        order.Confirm();

        Assert.Equal(OrderStatus.Confirmed, order.Status);
    }

    [Fact]
    public void Order_Confirm_ThrowsException_WhenOrderIsEmpty()
    {
        var order = Order.Create(_orderId, _customerId);

        var exception = Assert.Throws<InvalidOperationException>(() => order.Confirm());
        Assert.Contains("Cannot confirm an empty order.", exception.Message);
    }

    [Fact]
    public void Order_Confirm_ThrowsException_WhenOrderNotPending()
    {
        var order = Order.Create(_orderId, _customerId);
        order.AddItem(_testSku, _testPrice, _testQuantity);
        order.Confirm(); // First confirm
        order.Ship(); // Then ship

        var exception = Assert.Throws<InvalidOperationException>(() => order.Confirm());
        Assert.Contains(
            "Action not allowed. Order is in Shipped state, but must be Pending.",
            exception.Message
        );
    }

    [Fact]
    public void Order_Ship_ChangesStatusToShipped()
    {
        var order = Order.Create(_orderId, _customerId);
        order.AddItem(_testSku, _testPrice, _testQuantity);
        order.Confirm();

        order.Ship();

        Assert.Equal(OrderStatus.Shipped, order.Status);
    }

    [Fact]
    public void Order_Ship_ThrowsException_WhenOrderNotConfirmed()
    {
        var order = Order.Create(_orderId, _customerId);
        order.AddItem(_testSku, _testPrice, _testQuantity); // Order is Pending

        var exception = Assert.Throws<InvalidOperationException>(() => order.Ship());
        Assert.Contains(
            "Action not allowed. Order is in Pending state, but must be Confirmed.",
            exception.Message
        );
    }

    [Fact]
    public void Order_Cancel_ChangesStatusToCancelled()
    {
        var order = Order.Create(_orderId, _customerId);
        order.AddItem(_testSku, _testPrice, _testQuantity);

        order.Cancel();

        Assert.Equal(OrderStatus.Cancelled, order.Status);
    }

    [Fact]
    public void Order_Cancel_ThrowsException_WhenOrderShipped()
    {
        var order = Order.Create(_orderId, _customerId);
        order.AddItem(_testSku, _testPrice, _testQuantity);
        order.Confirm();
        order.Ship();

        var exception = Assert.Throws<InvalidOperationException>(() => order.Cancel());
        Assert.Contains("Cannot cancel an order that has already shipped.", exception.Message);
    }
}
