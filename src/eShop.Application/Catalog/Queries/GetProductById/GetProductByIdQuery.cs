using eShop.Domain.Catalog;
using eShop.Domain.SharedKernel.ValueObjects;
using MediatR;

namespace eShop.Applcation.Catalog.Queries;

public sealed record GetProductByIdQuery(ProductId Id) : IRequest<Product>;
