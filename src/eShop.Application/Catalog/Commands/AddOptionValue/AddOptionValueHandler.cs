using eShop.Applcation.SharedKernel;
using eShop.Domain.Catalog;
using eShop.Domain.SharedKernel.ValueObjects;
using MediatR;

namespace eShop.Applcation.Catalog.Commands.AddOptionValue;

public sealed class AddOptionValueHandler : IRequestHandler<AddOptionValueCommand>
{
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public AddOptionValueHandler(IProductRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddOptionValueCommand request, CancellationToken cancellationToken)
    {
        var product = await _repository.FindByIdAsync(request.ProductId, cancellationToken);
        if (product is null)
            throw new InvalidOperationException("Product not found.");

        product.AddOptionValue(
            request.ProductOptionId,
            new OptionValueId(Guid.NewGuid()),
            request.OptionValueName
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
