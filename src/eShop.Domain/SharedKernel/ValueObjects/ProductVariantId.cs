namespace eShop.Domain.SharedKernel.ValueObjects;

public readonly record struct ProductVariantId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
