namespace eShop.Domain.SharedKernel.ValueObjects;

public readonly record struct OptionValueId(Guid Value)
{
    public override string ToString() => Value.ToString();
}
