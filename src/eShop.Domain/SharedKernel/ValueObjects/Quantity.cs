namespace eShop.Domain.SharedKernel.ValueObjects;

public readonly record struct Quantity
{
    public int Value { get; }

    private Quantity(int value)
    {
        if (int.IsNegative(value))
            throw new ArgumentException("Quantity cannot be negative.", nameof(value));

        Value = value;
    }

    public static Quantity Create(int value) => new Quantity(value);

    public static Quantity Zero => new Quantity(0);

    public static Quantity operator +(Quantity left, int right) => new Quantity(left.Value + right);

    public static Quantity operator -(Quantity left, int right) => new Quantity(left.Value - right);

    public static Quantity operator +(Quantity left, Quantity right) =>
        new Quantity(left.Value + right.Value);

    public static Quantity operator -(Quantity left, Quantity right) =>
        new Quantity(left.Value - right.Value);

    public static bool operator <(Quantity left, Quantity right) => left.Value < right.Value;

    public static bool operator >(Quantity left, Quantity right) => left.Value > right.Value;

    public static bool operator <=(Quantity left, Quantity right) => left.Value <= right.Value;

    public static bool operator >=(Quantity left, Quantity right) => left.Value >= right.Value;
}
