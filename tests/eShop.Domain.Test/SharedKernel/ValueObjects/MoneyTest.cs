using eShop.Domain.SharedKernel.ValueObjects;

public sealed class MoneyTest
{
    [Fact]
    public void Create_ShouldSetPropertiesCorrectly()
    {
        decimal amount = 150.05m;
        string currency = "LKR";

        var money = Money.Create(amount, currency);

        Assert.Equal(amount, money.Amount);
        Assert.Equal(currency, money.Currency);
    }

    [Fact]
    public void Create_ShouldNormalizeCurrencyToUppercase()
    {
        var money = Money.Create(100, "lkr");

        Assert.Equal("LKR", money.Currency);
    }

    [Fact]
    public void Create_ShouldThrowArgumentNullException_WhenCurrencyIsInvalid()
    {
        Assert.Throws<ArgumentNullException>(() =>
            Money.Create(100, null!)
        );
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldThrowArgumentException_WhenCurrencyIsInvalid(string? invalidCurrency)
    {
        Assert.Throws<ArgumentException>(() =>
            Money.Create(100, invalidCurrency!)
        );
    }

    [Fact]
    public void Money_WithSameValues_ShouldBeEqual()
    {
        var a = Money.Create(100, "LKR");
        var b = Money.Create(100, "LKR");

        Assert.Equal(a, b);
        Assert.True(a == b);
    }

    [Fact]
    public void Money_WithDifferentValues_ShouldNotBeEqual()
    {
        var a = Money.Create(100, "LKR");
        var b = Money.Create(200, "LKR");
        var c = Money.Create(100, "USD");

        Assert.NotEqual(a, b);
        Assert.False(a == b);
        Assert.NotEqual(a, c);
        Assert.False(a == c);
    }

    [Fact]
    public void Addition_WithSame_Currency_ShouldReturnCorrectSum()
    {
        var a = Money.Create(20, "LKR");
        var b = Money.Create(25, "LKR");

        var result = a + b;

        Assert.Equal(45, result.Amount);
        Assert.Equal("LKR", result.Currency);
    }

    [Fact]
    public void Addition_WithDifferent_Currencies_ShouldThrowInvalidOperation()
    {
        var lkr = Money.Create(200, "LKR");
        var usd = Money.Create(200, "USD");

        Assert.Throws<InvalidOperationException>(() => lkr + usd);
    }

    [Fact]
    public void Subtraction_WithSame_Currency_ShouldReturnCorrectDifference()
    {
        var a = Money.Create(20, "LKR");
        var b = Money.Create(25, "LKR");

        var result = a - b;

        Assert.Equal(25, result.Amount);
        Assert.Equal("LKR", result.Currency);
    }

    [Fact]
    public void Subtraction_WithDifferent_Currencies_ShouldThrowInvalidOperation()
    {
        var lkr = Money.Create(200, "LKR");
        var usd = Money.Create(200, "USD");

        Assert.Throws<InvalidOperationException>(() => lkr - usd);
    }

    [Fact]
    public void Multiplication_ShouldReturnCorrectProduct()
    {
        var money = Money.Create(10, "LKR");
        decimal factor = 2.5m;

        var result = money * factor;

        Assert.Equal(25, result.Amount);
        Assert.Equal("LKR", result.Currency);
    }

    [Fact]
    public void Division_ShouldReturnCorrectQuotient()
    {
        var money = Money.Create(100, "LKR");
        decimal divisor = 4m;

        var result = money / divisor;

        Assert.Equal(25, result.Amount);
        Assert.Equal("LKR", result.Currency);
    }

    [Fact]
    public void Division_ByZero_ShouldThrowDivideByZeroException()
    {
        var money = Money.Create(100, "LKR");

        Assert.Throws<DivideByZeroException>(() => money / 0m);
    }

    [Fact]
    public void LessThanOperator_ShouldReturnTrue_WhenAmountIsLess()
    {
        var a = Money.Create(10, "LKR");
        var b = Money.Create(20, "LKR");

        Assert.True(a < b);
        Assert.False(b < a);
    }

    [Fact]
    public void GreaterThanOperator_ShouldReturnTrue_WhenAmountIsGreater()
    {
        var a = Money.Create(20, "LKR");
        var b = Money.Create(10, "LKR");

        Assert.True(a > b);
        Assert.False(b > a);
    }

    [Fact]
    public void LessThanOrEqualOperator_ShouldReturnTrue_WhenAmountIsLessOrEqual()
    {
        var a = Money.Create(10, "LKR");
        var b = Money.Create(20, "LKR");
        var c = Money.Create(10, "LKR");

        Assert.True(a <= b);
        Assert.True(a <= c);
        Assert.False(b <= a);
    }

    [Fact]
    public void GreaterThanOrEqualOperator_ShouldReturnTrue_WhenAmountIsGreaterOrEqual()
    {
        var a = Money.Create(20, "LKR");
        var b = Money.Create(10, "LKR");
        var c = Money.Create(20, "LKR");

        Assert.True(a >= b);
        Assert.True(a >= c);
        Assert.False(b >= a);
    }

    [Fact]
    public void Comparison_WithDifferentCurrencies_ShouldThrowInvalidOperation()
    {
        var lkr = Money.Create(100, "LKR");
        var usd = Money.Create(100, "USD");

        Assert.Throws<InvalidOperationException>(() => lkr < usd);
        Assert.Throws<InvalidOperationException>(() => lkr > usd);
        Assert.Throws<InvalidOperationException>(() => lkr <= usd);
        Assert.Throws<InvalidOperationException>(() => lkr >= usd);
    }

    [Fact]
    public void ToString_ShouldReturnFormattedString()
    {
        var money = Money.Create(1234.56m, "USD");
        Assert.Equal("1,234.56 USD", money.ToString());

        var moneyZero = Money.Create(0m, "LKR");
        Assert.Equal("0.00 LKR", moneyZero.ToString());

        var moneyLarge = Money.Create(1000000.12m, "EUR");
        Assert.Equal("1,000,000.12 EUR", moneyLarge.ToString());
    }

    [Fact]
    public void CompareTo_ShouldReturnCorrectValue()
    {
        var a = Money.Create(10, "LKR");
        var b = Money.Create(20, "LKR");
        var c = Money.Create(10, "LKR");

        Assert.True(a.CompareTo(b) < 0);
        Assert.True(b.CompareTo(a) > 0);
        Assert.True(a.CompareTo(c) == 0);
    }

    [Fact]
    public void CompareTo_WithDifferentCurrencies_ShouldThrowInvalidOperation()
    {
        var lkr = Money.Create(100, "LKR");
        var usd = Money.Create(100, "USD");

        Assert.Throws<InvalidOperationException>(() => lkr.CompareTo(usd));
    }

    [Fact]
    public void Zero_ShouldReturnMoneyWithZeroAmountAndCorrectCurrency()
    {
        var money = Money.Zero("USD");

        Assert.Equal(0, money.Amount);
        Assert.Equal("USD", money.Currency);
    }
}
