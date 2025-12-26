using System.Globalization;

namespace eShop.Domain.SharedKernel.ValueObjects;

public readonly record struct Money : IComparable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; } = null!;

    private Money(decimal amount, string currency)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(currency);

        Amount = amount;
        Currency = currency.Trim().ToUpperInvariant();
    }

    public static Money Create(decimal amount, string currency) => new(amount, currency);

    public static Money Zero(string currency) => new(0, currency);

    private static void EnsureSameCurrency(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException(
                $"Currency mismatch: Cannot operate on {left.Currency} and {right.Currency}."
            );
    }

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
        new(money.Amount * factor, money.Currency);

    public static Money operator /(Money money, decimal divisor) =>
        new(money.Amount / divisor, money.Currency);

    public int CompareTo(Money other)
    {
        EnsureSameCurrency(this, other);
        return Amount.CompareTo(other.Amount);
    }

    public static bool operator <(Money left, Money right) => left.CompareTo(right) < 0;

    public static bool operator >(Money left, Money right) => left.CompareTo(right) > 0;

    public static bool operator <=(Money left, Money right) => left.CompareTo(right) <= 0;

    public static bool operator >=(Money left, Money right) => left.CompareTo(right) >= 0;

    public override string ToString() =>
        $"{Amount.ToString("N2", CultureInfo.InvariantCulture)} {Currency}";
}