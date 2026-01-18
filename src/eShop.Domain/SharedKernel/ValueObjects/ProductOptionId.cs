namespace eShop.Domain.SharedKernel.ValueObjects;

public readonly record struct ProductOptionId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
