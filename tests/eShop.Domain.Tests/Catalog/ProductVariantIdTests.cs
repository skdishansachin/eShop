using eShop.Domain.Catalog;

namespace eShop.Domain.Tests.Catalog;

public class ProductVariantIdTests
{
    [Fact]
    public void ProductVariantId_Creation_WithGuid_ReturnsCorrectValue()
    {
        Guid guid = Guid.NewGuid();

        ProductVariantId productVariantId = new ProductVariantId(guid);

        Assert.Equal(guid, productVariantId.Value);
    }

    [Fact]
    public void ProductVariantId_Equality_WithSameGuids_ReturnsTrue()
    {
        Guid guid = Guid.NewGuid();
        ProductVariantId id1 = new ProductVariantId(guid);
        ProductVariantId id2 = new ProductVariantId(guid);

        Assert.Equal(id1, id2);
        Assert.True(id1 == id2);
        Assert.Equal(id1.GetHashCode(), id2.GetHashCode());
    }

    [Fact]
    public void ProductVariantId_Inequality_WithDifferentGuids_ReturnsFalse()
    {
        ProductVariantId id1 = new ProductVariantId(Guid.NewGuid());
        ProductVariantId id2 = new ProductVariantId(Guid.NewGuid());

        Assert.NotEqual(id1, id2);
        Assert.True(id1 != id2);
    }

    [Fact]
    public void ProductVariantId_ToString_ReturnsGuidString()
    {
        Guid guid = Guid.NewGuid();
        ProductVariantId productVariantId = new ProductVariantId(guid);

        string result = productVariantId.ToString();

        Assert.Equal(guid.ToString(), result);
    }
}
