using eShop.Domain.SharedKernel.ValueObjects;
using MediatR;

namespace eShop.Application.Inventory.Commands.AddStock;

public sealed record AddStockCommand(Sku Sku, Quantity QuantityOnHand, Quantity ReservedQuantity) : IRequest;
