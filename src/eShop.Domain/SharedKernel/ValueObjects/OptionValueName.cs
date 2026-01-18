namespace eShop.Domain.SharedKernel.ValueObjects;

public readonly record struct OptionValueName
{
    public string Value { get; }

    public OptionValueName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Option value name cannot be empty.");

        Value = value.Trim();
    }

    public static OptionValueName Create(string value) => new OptionValueName(value);

    public bool Equals(OptionValueName other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();

    public override string ToString() => Value;
}
