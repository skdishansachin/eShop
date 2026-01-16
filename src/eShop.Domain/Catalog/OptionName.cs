namespace eShop.Domain.Catalog;

public readonly record struct OptionName
{
    public string Value { get; }

    public OptionName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Option name cannot be empty.");

        Value = value.Trim();
    }

    public static OptionName Create(string value) => new OptionName(value);

    public bool Equals(OptionName other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();

    public override string ToString() => Value;
}
