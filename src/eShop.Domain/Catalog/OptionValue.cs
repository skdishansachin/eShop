namespace eShop.Domain.Catalog;

public sealed class OptionValue
{
    internal OptionValue(OptionValueId id, OptionValueName name)
    {
        Id = id;
        Name = name;
    }

    public OptionValueId Id { get; }
    public OptionValueName Name { get; private set; }
}
