using MediatR;
using eShop.Domain.SharedKernel.ValueObjects;

namespace eShop.Applcation.Catalog.Commands.AddProductVariant;

public sealed record AddProductVariantCommand(
        ProductId ProductId,
        Sku Sku,
        Money Price,
        Dictionary<ProductOptionId, OptionValueId> Selections
        ) : IRequest;
