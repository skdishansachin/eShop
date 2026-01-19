using eShop.Domain.SharedKernel.ValueObjects;

public class OrderItemIdTests
{
    [Fact]
    public void OrderItemId_WithValidGuid_CanBeInstantiated()
    {
        var guid = Guid.NewGuid();
        var orderItemId = new OrderItemId(guid);

        Assert.Equal(guid, orderItemId.Value);
    }

    [Fact]
    public void OrderItemId_WithSameGuids_AreEqual()
    {
        var guid = Guid.NewGuid();
        var orderItemId1 = new OrderItemId(guid);
        var orderItemId2 = new OrderItemId(guid);

        Assert.Equal(orderItemId1, orderItemId2);
        Assert.True(orderItemId1 == orderItemId2);
    }

    [Fact]
    public void OrderItemId_WithDifferentGuids_AreNotEqual()
    {
        var orderItemId1 = new OrderItemId(Guid.NewGuid());
        var orderItemId2 = new OrderItemId(Guid.NewGuid());

        Assert.NotEqual(orderItemId1, orderItemId2);
        Assert.True(orderItemId1 != orderItemId2);
    }

    [Fact]
    public void OrderItemId_ToString_ReturnsGuidString()
    {
        var guid = Guid.NewGuid();
        var orderItemId = new OrderItemId(guid);

        Assert.Equal(guid.ToString(), orderItemId.ToString());
    }

    [Fact]
    public void OrderItemId_WithEmptyGuid_CanBeInstantiated()
    {
        var guid = Guid.Empty;
        var orderItemId = new OrderItemId(guid);

        Assert.Equal(guid, orderItemId.Value);
    }
}
