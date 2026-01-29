namespace eShop.Domain.Catalog;

using eShop.Domain.SharedKernel.ValueObjects;

public sealed class ProductOption
{
    internal ProductOption(ProductOptionId id, OptionName name)
    {
        Id = id;
        Name = name;
    }

    public ProductOptionId Id { get; }
    public OptionName Name { get; private set; }

    private List<OptionValue> _values { get; } = new();
    public IReadOnlyCollection<OptionValue> Values => _values.AsReadOnly();

    public void AddValue(OptionValue option)
    {
        if (_values.Any(o => o == option))
            throw new InvalidOperationException("Value already exists for this option.");

        _values.Add(option);
    }

    public bool HasValue(OptionValueId valueId)
    {
        return _values.Any(v => v.Id == valueId);
    }
}
