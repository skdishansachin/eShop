using eShop.Applcation.Catalog.Queries;
using eShop.Domain.Catalog;
using eShop.Domain.SharedKernel.ValueObjects;
using Moq;

namespace eShop.Application.Tests.Catalog;

public class GetProductByIdHandlerTests
{
    private readonly Mock<IProductRepository> _repository;
    private readonly GetProductByIdHandler _handler;

    public GetProductByIdHandlerTests()
    {
        _repository = new Mock<IProductRepository>();
        _handler = new GetProductByIdHandler(_repository.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnProduct_WhenProductExists()
    {
        var productId = new ProductId(Guid.NewGuid());
        var query = new GetProductByIdQuery(productId);
        var cancellationToken = CancellationToken.None;
        var expectedProduct = Product.Create(productId, "Existing Product", "Description");

        _repository
            .Setup(r => r.FindByIdAsync(productId, cancellationToken))
            .ReturnsAsync(expectedProduct);

        var result = await _handler.Handle(query, cancellationToken);

        Assert.NotNull(result);
        Assert.Equal(expectedProduct, result);
        _repository.Verify(r => r.FindByIdAsync(productId, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenProductDoesNotExist()
    {
        var productId = new ProductId(Guid.NewGuid());
        var query = new GetProductByIdQuery(productId);
        var cancellationToken = CancellationToken.None;

        _repository
            .Setup(r => r.FindByIdAsync(productId, cancellationToken))
            .ReturnsAsync((Product?)null);

        var result = await _handler.Handle(query, cancellationToken);

        Assert.Null(result);
        _repository.Verify(r => r.FindByIdAsync(productId, cancellationToken), Times.Once);
    }
}
