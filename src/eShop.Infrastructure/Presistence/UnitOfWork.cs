using eShop.Applcation.SharedKernel;

namespace eShop.Infrastructure.Presistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
