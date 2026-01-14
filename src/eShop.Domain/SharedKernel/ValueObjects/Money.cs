namespace eShop.Domain.ValueObjects;

using System.Globalization;

public readonly record struct Money
{
    private const int CURRENCY_LENGHT = 3;

    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency) || currency.Length != CURRENCY_LENGHT)
            throw new ArgumentException("Currency must be a 3-letter ISO code.", nameof(currency));

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public static Money Create(decimal value, string currency) => new Money(value, currency);

    public static Money operator +(Money left, Money right)
    {
        EnsureSameCurrency(left, right);
        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public static Money operator -(Money left, Money right)
    {
        EnsureSameCurrency(left, right);
        return new Money(left.Amount - right.Amount, left.Currency);
    }

    public static Money operator *(Money money, decimal factor) =>
        new Money(money.Amount * factor, money.Currency);

    public static Money operator *(decimal factor, Money money) => money * factor;

    private static void EnsureSameCurrency(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException(
                $"Currencies must match ({left.Currency}, {right.Currency})."
            );
    }

    public override string ToString() =>
        $"{Currency} {Amount.ToString("F2", CultureInfo.InvariantCulture)}";
}