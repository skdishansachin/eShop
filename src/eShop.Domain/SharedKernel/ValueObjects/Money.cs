using System.Globalization;

namespace eShop.Domain.SharedKernel.ValueObjects;

public readonly record struct Money
{
    public decimal Amount { get; }

    private Money(decimal amount)
    {
        if (decimal.IsNegative(amount))
            throw new ArgumentException("Price cannot be negative value.", nameof(amount));
        Amount = amount;
    }

    public static Money Create(decimal amount) => new(amount);

    public static Money Zero() => new(0);

    public static Money operator +(Money left, Money right) => new(left.Amount + right.Amount);

    public static Money operator -(Money left, Money right) => new(left.Amount - right.Amount);

    public static Money operator *(Money money, decimal factor) => new(money.Amount * factor);

    public static Money operator *(decimal factor, Money money) => money * factor;

    public static Money operator /(Money money, decimal divisor) => new(money.Amount / divisor);

    public static Money operator /(Money money, int divisor) => new(money.Amount / divisor);

    public override string ToString() => Amount.ToString("F2", CultureInfo.InvariantCulture);
}
