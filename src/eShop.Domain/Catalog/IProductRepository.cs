using eShop.Domain.SharedKernel.ValueObjects;

namespace eShop.Domain.Catalog;

public interface IProductRepository
{
    Task<Product?> FindByIdAsync(ProductId id, CancellationToken cancellationToken);
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task<Product?> FindBySkuAsync(Sku sku, CancellationToken cancellationToken);
}
