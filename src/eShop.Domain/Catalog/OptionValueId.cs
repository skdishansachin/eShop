namespace eShop.Domain.Catalog;

public readonly record struct OptionValueId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
