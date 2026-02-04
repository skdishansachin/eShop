using eShop.Domain.Catalog;
using eShop.Domain.SharedKernel.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace eShop.Infrastructure.Presistence.Repositories;

public sealed class ProductRepository : IProductRepository
{

    private ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
    }

    public async Task<Product?> FindByIdAsync(ProductId id, CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}
