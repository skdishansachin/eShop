using eShop.Domain.SharedKernel.ValueObjects;

public class OrderIdTests
{
    [Fact]
    public void OrderId_WithValidGuid_CanBeInstantiated()
    {
        var guid = Guid.NewGuid();
        var orderId = new OrderId(guid);

        Assert.Equal(guid, orderId.Value);
    }

    [Fact]
    public void OrderId_WithSameGuids_AreEqual()
    {
        var guid = Guid.NewGuid();
        var orderId1 = new OrderId(guid);
        var orderId2 = new OrderId(guid);

        Assert.Equal(orderId1, orderId2);
        Assert.True(orderId1 == orderId2);
    }

    [Fact]
    public void OrderId_WithDifferentGuids_AreNotEqual()
    {
        var orderId1 = new OrderId(Guid.NewGuid());
        var orderId2 = new OrderId(Guid.NewGuid());

        Assert.NotEqual(orderId1, orderId2);
        Assert.True(orderId1 != orderId2);
    }

    [Fact]
    public void OrderId_ToString_ReturnsGuidString()
    {
        var guid = Guid.NewGuid();
        var orderId = new OrderId(guid);

        Assert.Equal(guid.ToString(), orderId.ToString());
    }

    [Fact]
    public void OrderId_WithEmptyGuid_CanBeInstantiated()
    {
        var guid = Guid.Empty;
        var orderId = new OrderId(guid);

        Assert.Equal(guid, orderId.Value);
    }
}
