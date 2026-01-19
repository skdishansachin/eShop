using eShop.Domain.Orders;
using eShop.Domain.SharedKernel.ValueObjects;

public class OrderTests
{
    [Fact]
    public void Order_Create_WithValidParameters_ReturnsOrderObject()
    {
        var id = new OrderId(Guid.NewGuid());
        var customerId = new CustomerId(Guid.NewGuid());

        var order = Order.Create(id, customerId);

        Assert.Equal(id, order.Id);
        Assert.Equal(customerId, order.CustomerId);
        Assert.Equal(Money.Create(0, "LKR"), order.TotalPrice);
        Assert.Equal(OrderStatus.Pending, order.Status);
    }

    [Fact]
    public void Order_AddItem_WithValidItem_AddsOrderItemToList()
    {
        var order = CreateOrder();

        order.AddItem(Sku.Create("SHIRT-RED-M"), Money.Create(1000, "LKR"), Quantity.Create(2));

        Assert.Single(order.Items);
        Assert.Equal(Money.Create(2000, "LKR"), order.TotalPrice);
    }

    [Fact]
    public void Order_AddItem_WithValidMultipleItems_AddsOrderItemToList()
    {
        var order = CreateOrder();

        order.AddItem(Sku.Create("SHIRT-RED-M"), Money.Create(1000, "LKR"), Quantity.Create(2));
        order.AddItem(Sku.Create("SHIRT-RED-S"), Money.Create(1000, "LKR"), Quantity.Create(1));

        Assert.Equal(2, order.Items.Count);
        Assert.Equal(Money.Create(3000, "LKR"), order.TotalPrice);
    }

    [Fact]
    public void Order_AddItem_WithDifferentCurrencyItems_ThrowsInvalidOperationException()
    {
        var order = CreateOrder();
        order.AddItem(Sku.Create("SHIRT-RED-M"), Money.Create(1000, "LKR"), Quantity.Create(2));

        Assert.Throws<InvalidOperationException>(() =>
        {
            order.AddItem(Sku.Create("SHIRT-RED-S"), Money.Create(1000, "USD"), Quantity.Create(1));
        });
    }

    [Fact]
    public void Order_AddItem_WithDuplicateSku_ThrowsInvalidOperationException()
    {
        var order = CreateOrder();
        var sku = Sku.Create("SHIRT-RED-M");

        order.AddItem(sku, Money.Create(1000, "LKR"), Quantity.Create(2));

        Assert.Throws<InvalidOperationException>(() =>
            order.AddItem(sku, Money.Create(1200, "LKR"), Quantity.Create(1))
        );
    }

    [Fact]
    public void Order_Confirm_WhenOrderIsPending_UpdateStatusToConfirm()
    {
        var order = CreateOrderWithItems();

        order.Confirm();

        Assert.Equal(OrderStatus.Confirmed, order.Status);
    }

    [Theory]
    [InlineData(OrderStatus.Shipped)]
    [InlineData(OrderStatus.Cancelled)]
    public void Order_Confirm_WhenInInvalidState_ThrowsInvalidOperationException(OrderStatus status)
    {
        Order order = status switch
        {
            OrderStatus.Shipped => CreateShippedOrder(),
            OrderStatus.Cancelled => CreateCanceledOrder(),
            _ => throw new ArgumentException("Status not covered"),
        };

        Assert.Throws<InvalidOperationException>(() => order.Confirm());
    }

    [Fact]
    public void Order_Ship_WhenOrderIsConfirmed_UpdateStatusToShip()
    {
        var order = CreateOrderWithItems();

        order.Confirm();
        order.Ship();

        Assert.Equal(OrderStatus.Shipped, order.Status);
    }

    [Theory]
    [InlineData(OrderStatus.Pending)]
    [InlineData(OrderStatus.Cancelled)]
    public void Order_Ship_WhenInInvalidState_ThrowsInvalidOperationException(OrderStatus status)
    {
        Order order = status switch
        {
            OrderStatus.Pending => CreateOrder(),
            OrderStatus.Cancelled => CreateCanceledOrder(),
            _ => throw new ArgumentException("Status not covered"),
        };

        Assert.Throws<InvalidOperationException>(() => order.Ship());
    }

    [Fact]
    public void Order_Cancel_WhenOrderIsPending_UpdateStatusToCancel()
    {
        var order = CreateOrderWithItems();

        order.Cancel();

        Assert.Equal(OrderStatus.Cancelled, order.Status);
    }

    [Theory]
    [InlineData(OrderStatus.Shipped)]
    public void Order_Cancel_WhenInInvalidState_ThrowsInvalidOperationException(OrderStatus status)
    {
        Order order = status switch
        {
            OrderStatus.Shipped => CreateShippedOrder(),
            _ => throw new ArgumentException("Status not covered"),
        };

        Assert.Throws<InvalidOperationException>(() => order.Cancel());
    }

    // HELPER METHODS

    private Order CreateOrder()
    {
        return Order.Create(new OrderId(Guid.NewGuid()), new CustomerId(Guid.NewGuid()));
    }

    private Order CreateOrderWithItems()
    {
        var order = CreateOrder();
        order.AddItem(Sku.Create("SHIRT-RED-M"), Money.Create(1000, "LKR"), Quantity.Create(2));
        order.AddItem(Sku.Create("SHIRT-RED-S"), Money.Create(1000, "LKR"), Quantity.Create(1));
        return order;
    }

    private Order CreateCanceledOrder()
    {
        var order = CreateOrderWithItems();
        order.Cancel();

        return order;
    }

    private Order CreateShippedOrder()
    {
        var order = CreateOrderWithItems();
        order.Confirm();
        order.Ship();

        return order;
    }
}
