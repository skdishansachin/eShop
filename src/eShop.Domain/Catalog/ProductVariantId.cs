namespace eShop.Domain.Catalog;

public readonly record struct ProductVariantId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
