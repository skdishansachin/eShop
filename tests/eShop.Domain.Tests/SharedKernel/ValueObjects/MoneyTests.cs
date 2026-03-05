using eShop.Domain.SharedKernel.ValueObjects;

namespace eShop.Domain.Tests.SharedKernel.ValueObjects;

public sealed class MoneyTests
{
    [Fact]
    public void Create_WithPositiveAmount_ShouldSucceed()
    {
        var amount = 100.50m;
        var money = Money.Create(amount);
        Assert.Equal(amount, money.Amount);
    }

    [Fact]
    public void Create_WithZeroAmount_ShouldSucceed()
    {
        var money = Money.Create(0);
        Assert.Equal(0, money.Amount);
    }

    [Fact]
    public void Create_WithNegativeAmount_ShouldThrow()
    {
        var negativeAmount = -100m;
        Assert.Throws<ArgumentException>(() => Money.Create(negativeAmount));
    }

    [Fact]
    public void Zero_ShouldReturnMoneyWithZeroAmount()
    {
        var money = Money.Zero();
        Assert.Equal(0, money.Amount);
    }

    [Fact]
    public void AdditionOperator_ShouldReturnCorrectSum()
    {
        var money1 = Money.Create(100m);
        var money2 = Money.Create(50m);
        var result = money1 + money2;
        Assert.Equal(150m, result.Amount);
    }

    [Fact]
    public void SubtractionOperator_ShouldReturnCorrectDifference()
    {
        var money1 = Money.Create(100m);
        var money2 = Money.Create(50m);
        var result = money1 - money2;
        Assert.Equal(50m, result.Amount);
    }

    [Fact]
    public void SubtractionOperator_WithLargerRightOperand_ShouldThrow()
    {
        var money1 = Money.Create(50m);
        var money2 = Money.Create(100m);
        Assert.Throws<ArgumentException>(() => money1 - money2);
    }

    [Fact]
    public void MultiplicationOperator_WithDecimalFactor_ShouldReturnCorrectProduct()
    {
        var money = Money.Create(100m);
        var factor = 1.5m;
        var result = money * factor;
        Assert.Equal(150m, result.Amount);
    }

    [Fact]
    public void MultiplicationOperator_WithCommutativeDecimalFactor_ShouldReturnCorrectProduct()
    {
        var money = Money.Create(100m);
        var factor = 1.5m;
        var result = factor * money;
        Assert.Equal(150m, result.Amount);
    }

    [Fact]
    public void DivisionOperator_WithDecimalDivisor_ShouldReturnCorrectQuotient()
    {
        var money = Money.Create(100m);
        var divisor = 2m;
        var result = money / divisor;
        Assert.Equal(50m, result.Amount);
    }

    [Fact]
    public void DivisionOperator_WithIntDivisor_ShouldReturnCorrectQuotient()
    {
        var money = Money.Create(100m);
        var divisor = 4;
        var result = money / divisor;
        Assert.Equal(25m, result.Amount);
    }

    [Fact]
    public void ToString_ShouldFormatWithTwoDecimalPlaces()
    {
        var money = Money.Create(123.456m);
        var result = money.ToString();
        Assert.Equal("123.46", result);
    }

    [Fact]
    public void Equality_ShouldBeValueBased()
    {
        var money1 = Money.Create(100m);
        var money2 = Money.Create(100m);
        Assert.Equal(money1, money2);
        Assert.True(money1 == money2);
        Assert.False(money1 != money2);
    }

    [Fact]
    public void Equality_WithDifferentAmounts_ShouldNotBeEqual()
    {
        var money1 = Money.Create(100m);
        var money2 = Money.Create(50m);
        Assert.NotEqual(money1, money2);
        Assert.False(money1 == money2);
        Assert.True(money1 != money2);
    }
}
