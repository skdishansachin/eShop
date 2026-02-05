using MediatR;
using eShop.Domain.Catalog;
using eShop.Applcation.SharedKernel;

namespace eShop.Applcation.Catalog.Commands.AddProductVariant;


public sealed class AddProductVariant : IRequestHandler<AddProductVariantCommand>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddProductVariant(IProductRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddProductVariantCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.FindByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            throw new InvalidOperationException("Product not found.");

        var existing = await _repository.FindBySkuAsync(
            request.Sku,
            cancellationToken);

        if (existing is not null)
            throw new InvalidOperationException(
                $"SKU {request.Sku} already exists.");

        product.AddVariant(
            request.Sku,
            request.Price,
            request.Selections);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

