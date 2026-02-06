using eShop.Domain.SharedKernel.ValueObjects;
using MediatR;

namespace eShop.Applcation.Catalog.Commands.AddProductVariant;

public sealed record AddProductVariantCommand(
    ProductId ProductId,
    Sku Sku,
    Money Price,
    Dictionary<ProductOptionId, OptionValueId> Selections
) : IRequest;
