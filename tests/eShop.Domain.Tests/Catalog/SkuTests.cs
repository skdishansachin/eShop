using eShop.Domain.Catalog;

namespace eShop.Domain.Tests.Catalog;

public class SkuTests
{
    [Fact]
    public void Sku_Create_WithValidValue_ReturnsSkuObject()
    {
        string value = "SKU-12345";

        Sku sku = Sku.Create(value);

        Assert.Equal(value, sku.Value);
    }

    [Fact]
    public void Sku_Constructor_WithNullValue_ThrowsArgumentException()
    {
        string value = null!;

        Assert.Throws<ArgumentException>(() => Sku.Create(value));
    }

    [Fact]
    public void Sku_Constructor_WithEmptyValue_ThrowsArgumentException()
    {
        string value = "";

        Assert.Throws<ArgumentException>(() => Sku.Create(value));
    }

    [Fact]
    public void Sku_Constructor_WithWhitespaceValue_ThrowsArgumentException()
    {
        string value = "   ";

        Assert.Throws<ArgumentException>(() => Sku.Create(value));
    }

    [Fact]
    public void Sku_Equality_WithSameValue_ReturnsTrue()
    {
        Sku sku1 = Sku.Create("SKU-ABC");
        Sku sku2 = Sku.Create("SKU-ABC");

        Assert.True(sku1.Equals(sku2));
        Assert.True(sku1 == sku2);
        Assert.Equal(sku1.GetHashCode(), sku2.GetHashCode());
    }

    [Fact]
    public void Sku_Inequality_WithDifferentValue_ReturnsFalse()
    {
        Sku sku1 = Sku.Create("SKU-ABC");
        Sku sku2 = Sku.Create("SKU-DEF");

        Assert.False(sku1.Equals(sku2));
        Assert.True(sku1 != sku2);
    }

    [Fact]
    public void Sku_ToString_ReturnsValue()
    {
        string value = "PROD-XYZ";
        Sku sku = Sku.Create(value);

        string result = sku.ToString();

        Assert.Equal(value, result);
    }
}
