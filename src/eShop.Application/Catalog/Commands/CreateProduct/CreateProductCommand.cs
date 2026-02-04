using eShop.Domain.SharedKernel.ValueObjects;
using MediatR;

namespace eShop.Applcation.Catalog.Commands.CreateProduct;

public sealed record CreateProductCommand(string Title, string Description) : IRequest<ProductId>;
