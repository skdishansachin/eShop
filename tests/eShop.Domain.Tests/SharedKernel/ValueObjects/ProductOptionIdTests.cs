using eShop.Domain.SharedKernel.ValueObjects;

namespace eShop.Domain.Tests.SharedKernel.ValueObjects;

public class ProductOptionIdTests
{
    [Fact]
    public void ProductOptionId_Creation_WithGuid_ReturnsCorrectValue()
    {
        Guid guid = Guid.NewGuid();

        ProductOptionId productOptionId = new ProductOptionId(guid);

        Assert.Equal(guid, productOptionId.Value);
    }

    [Fact]
    public void ProductOptionId_Equality_WithSameGuids_ReturnsTrue()
    {
        Guid guid = Guid.NewGuid();
        ProductOptionId id1 = new ProductOptionId(guid);
        ProductOptionId id2 = new ProductOptionId(guid);

        Assert.Equal(id1, id2);
        Assert.True(id1 == id2);
        Assert.Equal(id1.GetHashCode(), id2.GetHashCode());
    }

    [Fact]
    public void ProductOptionId_Inequality_WithDifferentGuids_ReturnsFalse()
    {
        ProductOptionId id1 = new ProductOptionId(Guid.NewGuid());
        ProductOptionId id2 = new ProductOptionId(Guid.NewGuid());

        Assert.NotEqual(id1, id2);
        Assert.True(id1 != id2);
    }

    [Fact]
    public void ProductOptionId_ToString_ReturnsGuidString()
    {
        Guid guid = Guid.NewGuid();
        ProductOptionId productOptionId = new ProductOptionId(guid);

        string result = productOptionId.ToString();

        Assert.Equal(guid.ToString(), result);
    }
}
