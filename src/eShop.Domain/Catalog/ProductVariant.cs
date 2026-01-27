using eShop.Domain.SharedKernel.ValueObjects;

namespace eShop.Domain.Catalog;

public sealed class ProductVariant
{
    private ProductVariant() { }

    internal ProductVariant(ProductVariantId id, Sku sku, Money price, List<OptionValueId> values)
    {
        Id = id;
        Sku = sku;
        Price = price;
        _values = values;
    }

    public ProductVariantId Id { get; }
    public Sku Sku { get; private set; }
    public Money Price { get; private set; }

    private List<OptionValueId> _values { get; } = new();
    public IReadOnlyCollection<OptionValueId> Values => _values.AsReadOnly();

    private List<ProductVariantSelection> _selections { get; } = new();
    public IReadOnlyCollection<ProductVariantSelection> Selections => _selections.AsReadOnly();

    internal bool HasSameSelections(IEnumerable<OptionValueId> other)
    {
        if (_values.Count != other.Count())
            return false;

        // Checks if every ID in the incoming list exists in our internal list
        return other.All(id => _values.Contains(id));
    }

    internal Money UpdatePrice(Money newPrice)
    {
        if (Price == newPrice)
            return Price;

        var oldPrice = Price;
        Price = newPrice;

        return oldPrice;
    }
}
