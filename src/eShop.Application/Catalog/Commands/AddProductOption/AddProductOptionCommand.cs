using eShop.Domain.SharedKernel.ValueObjects;
using MediatR;

namespace eShop.Applcation.Catalog.Commands.AddProductOption;

public sealed record AddProductOptionCommand(
    ProductId ProductId,
    ProductOptionId ProductOptionId,
    OptionName OptionName
) : IRequest;
