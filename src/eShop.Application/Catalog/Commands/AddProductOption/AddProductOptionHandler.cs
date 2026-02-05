using eShop.Applcation.SharedKernel;
using eShop.Domain.Catalog;
using eShop.Domain.SharedKernel.ValueObjects;
using MediatR;

namespace eShop.Applcation.Catalog.Commands.AddProductOption;

public sealed class AddProductOptionHandler : IRequestHandler<AddProductOptionCommand>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddProductOptionHandler(IProductRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddProductOptionCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.FindByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            throw new InvalidOperationException("Product not found");

        product.AddOption(request.ProductOptionId, request.OptionName);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

    }
}
