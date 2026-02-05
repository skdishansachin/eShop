using eShop.Domain.SharedKernel.ValueObjects;
using MediatR;

namespace eShop.Applcation.Catalog.Commands.AddOptionValue;

public sealed record AddOptionValueCommand(
    ProductId ProductId,
    ProductOptionId ProductOptionId,
    OptionValueName OptionValueName
) : IRequest;
