namespace eShop.Domain.Catalog;

public readonly record struct ProductOptionId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
