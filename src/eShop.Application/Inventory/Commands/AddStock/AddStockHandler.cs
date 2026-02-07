using eShop.Applcation.SharedKernel;
using eShop.Domain.Catalog;
using MediatR;

namespace eShop.Application.Inventory.Commands.AddStock;

public sealed class AddStockHandler : IRequestHandler<AddStockCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProductRepository _productRepository;

    public AddStockHandler(IUnitOfWork unitOfWork, IProductRepository productRepository)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
    }

    public async Task Handle(AddStockCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
