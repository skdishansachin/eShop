namespace eShop.Domain.SharedKernel.ValueObjects;

public readonly record struct OrderId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
