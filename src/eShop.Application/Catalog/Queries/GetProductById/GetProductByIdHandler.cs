using eShop.Domain.Catalog;
using MediatR;

namespace eShop.Applcation.Catalog.Queries;

public sealed class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product?>
{
    private readonly IProductRepository _repository;

    public GetProductByIdHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Product?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        return await _repository.FindByIdAsync(request.Id, cancellationToken);
    }
}
