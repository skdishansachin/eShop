using System;
using System.Globalization;
using eShop.Domain.SharedKernel.ValueObjects;
using Xunit;

namespace eShop.Domain.Tests.SharedKernel.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Money_Create_WithValidInput_ReturnsMoneyObject()
    {
        decimal amount = 100.50m;
        string currency = "USD";

        Money money = Money.Create(amount, currency);

        Assert.Equal(amount, money.Amount);
        Assert.Equal(currency, money.Currency);
    }

    [Fact]
    public void Money_Create_WithLowercaseCurrency_ConvertsToUppercase()
    {
        decimal amount = 10.00m;
        string currency = "eur";

        Money money = Money.Create(amount, currency);

        Assert.Equal("EUR", money.Currency);
    }

    [Fact]
    public void Money_Create_WithZeroAmount_ReturnsMoneyObject()
    {
        decimal amount = 0m;
        string currency = "GBP";

        Money money = Money.Create(amount, currency);

        Assert.Equal(amount, money.Amount);
        Assert.Equal(currency, money.Currency);
    }

    [Fact]
    public void Money_Create_WithNegativeAmount_ReturnsMoneyObject()
    {
        decimal amount = -50.25m;
        string currency = "JPY";

        Money money = Money.Create(amount, currency);

        Assert.Equal(amount, money.Amount);
        Assert.Equal(currency, money.Currency);
    }

    [Fact]
    public void Money_Create_WithNullCurrency_ThrowsArgumentException()
    {
        decimal amount = 100m;
        string currency = null!;

        Assert.Throws<ArgumentException>(() => Money.Create(amount, currency));
    }

    [Fact]
    public void Money_Create_WithEmptyCurrency_ThrowsArgumentException()
    {
        decimal amount = 100m;
        string currency = "";

        Assert.Throws<ArgumentException>(() => Money.Create(amount, currency));
    }

    [Fact]
    public void Money_Create_WithWhitespaceCurrency_ThrowsArgumentException()
    {
        decimal amount = 100m;
        string currency = "   ";

        Assert.Throws<ArgumentException>(() => Money.Create(amount, currency));
    }

    [Fact]
    public void Money_Create_WithTooShortCurrency_ThrowsArgumentException()
    {
        decimal amount = 100m;
        string currency = "US";

        Assert.Throws<ArgumentException>(() => Money.Create(amount, currency));
    }

    [Fact]
    public void Money_Create_WithTooLongCurrency_ThrowsArgumentException()
    {
        decimal amount = 100m;
        string currency = "USDA";

        Assert.Throws<ArgumentException>(() => Money.Create(amount, currency));
    }

    [Fact]
    public void Money_Addition_WithSameCurrency_ReturnsCorrectSum()
    {
        Money money1 = Money.Create(100m, "USD");
        Money money2 = Money.Create(50m, "USD");

        Money result = money1 + money2;

        Assert.Equal(150m, result.Amount);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public void Money_Addition_WithDifferentCurrencies_ThrowsInvalidOperationException()
    {
        Money money1 = Money.Create(100m, "USD");
        Money money2 = Money.Create(50m, "EUR");

        Assert.Throws<InvalidOperationException>(() => money1 + money2);
    }

    [Fact]
    public void Money_Subtraction_WithSameCurrency_ReturnsCorrectDifference()
    {
        Money money1 = Money.Create(100m, "USD");
        Money money2 = Money.Create(50m, "USD");

        Money result = money1 - money2;

        Assert.Equal(50m, result.Amount);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public void Money_Subtraction_WithDifferentCurrencies_ThrowsInvalidOperationException()
    {
        Money money1 = Money.Create(100m, "USD");
        Money money2 = Money.Create(50m, "EUR");

        Assert.Throws<InvalidOperationException>(() => money1 - money2);
    }

    [Fact]
    public void Money_Multiplication_ByFactor_ReturnsCorrectProduct()
    {
        Money money = Money.Create(100m, "USD");
        decimal factor = 2.5m;

        Money result = money * factor;

        Assert.Equal(250m, result.Amount);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public void Money_Multiplication_FactorByMoney_ReturnsCorrectProduct()
    {
        Money money = Money.Create(100m, "USD");
        decimal factor = 2.5m;

        Money result = factor * money;

        Assert.Equal(250m, result.Amount);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public void Money_Equality_WithSameValues_ReturnsTrue()
    {
        Money money1 = Money.Create(100m, "USD");
        Money money2 = Money.Create(100m, "USD");

        Assert.True(money1 == money2);
        Assert.Equal(money1, money2);
        Assert.Equal(money1.GetHashCode(), money2.GetHashCode());
    }

    [Fact]
    public void Money_Inequality_WithDifferentAmounts_ReturnsFalse()
    {
        Money money1 = Money.Create(100m, "USD");
        Money money2 = Money.Create(50m, "USD");

        Assert.True(money1 != money2);
        Assert.NotEqual(money1, money2);
    }

    [Fact]
    public void Money_Inequality_WithDifferentCurrencies_ReturnsFalse()
    {
        Money money1 = Money.Create(100m, "USD");
        Money money2 = Money.Create(100m, "EUR");

        Assert.True(money1 != money2);
        Assert.NotEqual(money1, money2);
    }

    [Fact]
    public void Money_ToString_ReturnsCorrectFormat()
    {
        Money money = Money.Create(123.45m, "USD");

        string result = money.ToString();

        Assert.Equal("USD 123.45", result);
    }

    [Fact]
    public void Money_ToString_WithDifferentAmount_ReturnsCorrectFormat()
    {
        Money money = Money.Create(123.456m, "USD");

        string result = money.ToString();

        Assert.Equal("USD 123.46", result);
    }
}
