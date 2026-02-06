using eShop.Applcation.Catalog.Commands.CreateProduct;
using eShop.Applcation.SharedKernel;
using eShop.Domain.Catalog;
using eShop.Domain.SharedKernel.ValueObjects;
using Moq;

namespace eShop.Application.Tests.Catalog;

public class CreateProductHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateProductHandler(_productRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateProductWithoutVariantAndReturnProductId_WhenOnlyTitleAndDescriptionAreProvided()
    {
        var command = new CreateProductCommand("Test Product", "Description for test product");
        var cancellationToken = CancellationToken.None;

        _productRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, cancellationToken);

        Assert.IsType<ProductId>(result);
        _productRepositoryMock.Verify(
            r =>
                r.AddAsync(
                    It.Is<Product>(p =>
                        p.Title == command.Title && p.Description == command.Description
                    ),
                    cancellationToken
                ),
            Times.Once
        );
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(cancellationToken), Times.Once);
        _productRepositoryMock.Verify(
            r => r.FindBySkuAsync(It.IsAny<Sku>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateProductWithVariantAndReturnProductId_WhenPriceAndSkuAreProvided()
    {
        var price = Money.Create(100, "USD");
        var sku = Sku.Create("SKU123");
        var command = new CreateProductCommand(
            "Test Product With Variant",
            "Description",
            price,
            sku
        );
        var cancellationToken = CancellationToken.None;

        _productRepositoryMock
            .Setup(r => r.FindBySkuAsync(sku, cancellationToken))
            .ReturnsAsync((Product?)null); // SKU does not exist
        _productRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, cancellationToken);

        Assert.IsType<ProductId>(result);
        _productRepositoryMock.Verify(
            r =>
                r.AddAsync(
                    It.Is<Product>(p =>
                        p.Title == command.Title
                        && p.Description == command.Description
                        && p.Variants.Any(v => v.Sku == sku && v.Price == price)
                    ),
                    cancellationToken
                ),
            Times.Once
        );
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(cancellationToken), Times.Once);
        _productRepositoryMock.Verify(r => r.FindBySkuAsync(sku, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenPriceIsProvidedButSkuIsMissing()
    {
        var price = Money.Create(100, "USD");
        var command = new CreateProductCommand("Test Product", "Description", price, null);
        var cancellationToken = CancellationToken.None;

        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _handler.Handle(command, cancellationToken)
        );
        Assert.Equal("SKU is required for simple products.", exception.Message);
        _productRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _productRepositoryMock.Verify(
            r => r.FindBySkuAsync(It.IsAny<Sku>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenPriceAndExistingSkuAreProvided()
    {
        var price = Money.Create(100, "USD");
        var sku = Sku.Create("SKU456");
        var command = new CreateProductCommand("Test Product", "Description", price, sku);
        var cancellationToken = CancellationToken.None;
        var existingProduct = Product.Create(
            new ProductId(Guid.NewGuid()),
            "Existing Product",
            "Existing Description"
        );

        _productRepositoryMock
            .Setup(r => r.FindBySkuAsync(sku, cancellationToken))
            .ReturnsAsync(existingProduct); // SKU already exists

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _handler.Handle(command, cancellationToken)
        );
        Assert.Equal($"SKU {sku} already exists.", exception.Message);
        _productRepositoryMock.Verify(
            r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _productRepositoryMock.Verify(r => r.FindBySkuAsync(sku, cancellationToken), Times.Once);
    }
}
