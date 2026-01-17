using System;

using eShop.Domain.Catalog;
using eShop.Domain.SharedKernel.ValueObjects;

using Xunit;

namespace eShop.Domain.Tests.Catalog;

public class ProductIdTests
{
    [Fact]
    public void ProductId_Creation_WithGuid_ReturnsCorrectValue()
    {
        Guid guid = Guid.NewGuid();

        ProductId productId = new ProductId(guid);

        Assert.Equal(guid, productId.Value);
    }

    [Fact]
    public void ProductId_Equality_WithSameGuids_ReturnsTrue()
    {
        Guid guid = Guid.NewGuid();
        ProductId id1 = new ProductId(guid);
        ProductId id2 = new ProductId(guid);

        Assert.Equal(id1, id2);
        Assert.True(id1 == id2);
        Assert.Equal(id1.GetHashCode(), id2.GetHashCode());
    }

    [Fact]
    public void ProductId_Inequality_WithDifferentGuids_ReturnsFalse()
    {
        ProductId id1 = new ProductId(Guid.NewGuid());
        ProductId id2 = new ProductId(Guid.NewGuid());

        Assert.NotEqual(id1, id2);
        Assert.True(id1 != id2);
    }

    [Fact]
    public void ProductId_ToString_ReturnsGuidString()
    {
        Guid guid = Guid.NewGuid();
        ProductId productId = new ProductId(guid);

        string result = productId.ToString();

        Assert.Equal(guid.ToString(), result);
    }
}
