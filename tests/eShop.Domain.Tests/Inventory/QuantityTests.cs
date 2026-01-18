using eShop.Domain.Inventory;

namespace eShop.Domain.Tests.Inventory;

public class QuantityTests
{
    [Fact]
    public void Create_WithNegativeValue_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Quantity.Create(-1));
    }

    [Fact]
    public void Create_WithPositiveValue_ReturnsQuantity()
    {
        var quantity = Quantity.Create(1);
        Assert.Equal(1, quantity.Value);
    }

    [Fact]
    public void Zero_ReturnsQuantityWithZeroValue()
    {
        var zero = Quantity.Zero;
        Assert.Equal(0, zero.Value);
    }

    [Fact]
    public void Addition_WithInt_ReturnsCorrectQuantity()
    {
        var quantity = Quantity.Create(5);
        var result = quantity + 5;
        Assert.Equal(10, result.Value);
    }

    [Fact]
    public void Subtraction_WithInt_ReturnsCorrectQuantity()
    {
        var quantity = Quantity.Create(10);
        var result = quantity - 5;
        Assert.Equal(5, result.Value);
    }
    
    [Fact]
    public void Subtraction_WithInt_ToNegative_ThrowsArgumentException()
    {
        var quantity = Quantity.Create(5);
        Assert.Throws<ArgumentException>(() => quantity - 10);
    }

    [Fact]
    public void Addition_WithQuantity_ReturnsCorrectQuantity()
    {
        var q1 = Quantity.Create(5);
        var q2 = Quantity.Create(5);
        var result = q1 + q2;
        Assert.Equal(10, result.Value);
    }

    [Fact]
    public void Subtraction_WithQuantity_ReturnsCorrectQuantity()
    {
        var q1 = Quantity.Create(10);
        var q2 = Quantity.Create(5);
        var result = q1 - q2;
        Assert.Equal(5, result.Value);
    }
    
    [Fact]
    public void Subtraction_WithQuantity_ToNegative_ThrowsArgumentException()
    {
        var q1 = Quantity.Create(5);
        var q2 = Quantity.Create(10);
        Assert.Throws<ArgumentException>(() => q1 - q2);
    }

    [Fact]
    public void Comparison_Operators_WorkCorrectly()
    {
        var q5 = Quantity.Create(5);
        var q10 = Quantity.Create(10);

        Assert.True(q5 < q10);
        Assert.False(q10 < q5);

        Assert.True(q10 > q5);
        Assert.False(q5 > q10);

        Assert.True(q5 <= q10);
        Assert.True(q5 <= Quantity.Create(5));

        Assert.True(q10 >= q5);
        Assert.True(q10 >= Quantity.Create(10));
    }
    
    [Fact]
    public void Quantity_Equality_WithSameValue_ReturnsTrue()
    {
        var q1 = Quantity.Create(10);
        var q2 = Quantity.Create(10);

        Assert.True(q1.Equals(q2));
        Assert.True(q1 == q2);
        Assert.Equal(q1.GetHashCode(), q2.GetHashCode());
    }

    [Fact]
    public void Quantity_Inequality_WithDifferentValue_ReturnsFalse()
    {
        var q1 = Quantity.Create(10);
        var q2 = Quantity.Create(20);

        Assert.False(q1.Equals(q2));
        Assert.True(q1 != q2);
    }
}
