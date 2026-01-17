namespace eShop.Domain.SharedKernel.ValueObjects;

public readonly record struct ProductId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
