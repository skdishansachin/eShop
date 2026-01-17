namespace eShop.Domain.Catalog;

public readonly record struct Sku
{
    public string Value { get; }

    private Sku(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Sku cannot be empty.", nameof(value));

        Value = value;
    }

    public static Sku Create(string value) => new Sku(value);

    public override string ToString() => Value;
}
