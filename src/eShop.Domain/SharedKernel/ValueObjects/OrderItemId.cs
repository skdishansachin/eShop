namespace eShop.Domain.SharedKernel.ValueObjects;

public readonly record struct OrderItemId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
