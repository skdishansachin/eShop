using eShop.Applcation.SharedKernel;
using eShop.Domain.Catalog;
using eShop.Infrastructure.Presistence;
using eShop.Infrastructure.Presistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eShop.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IProductRepository, ProductRepository>();

        return services;
    }
}
