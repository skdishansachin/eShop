using eShop.Domain.SharedKernel.ValueObjects;

namespace eShop.Domain.Catalog;

public sealed class ProductVariantSelection
{
    private ProductVariantSelection() { }

    internal ProductVariantSelection(ProductOptionId optionId, OptionValueId valueId)
    {
        ProductOptionId = optionId;
        OptionValueId = valueId;
    }

    public ProductOptionId ProductOptionId { get; }
    public OptionValueId OptionValueId { get; }
}
