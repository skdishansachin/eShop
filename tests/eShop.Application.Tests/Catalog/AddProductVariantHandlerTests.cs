using eShop.Applcation.Catalog.Commands.AddProductVariant;
using eShop.Applcation.SharedKernel;
using eShop.Domain.Catalog;
using eShop.Domain.SharedKernel.ValueObjects;
using Moq;

namespace eShop.Application.Tests.Catalog;

public class AddProductVariantHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AddProductVariant _handler;

    public AddProductVariantHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new AddProductVariant(_productRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddProductVariant_WhenProductAndSkuAreValid()
    {
        var productId = new ProductId(Guid.NewGuid());
        var sku = Sku.Create("VAR001");
        var price = Money.Create(50, "USD");
        var selections = new Dictionary<ProductOptionId, OptionValueId>();
        var command = new AddProductVariantCommand(productId, sku, price, selections);
        var cancellationToken = CancellationToken.None;

        var product = Product.Create(productId, "Test Product", "Description");
        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, cancellationToken))
            .ReturnsAsync(product);
        _productRepositoryMock
            .Setup(r => r.FindBySkuAsync(sku, cancellationToken))
            .ReturnsAsync((Product?)null); // SKU does not exist
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(cancellationToken)).ReturnsAsync(1);

        await _handler.Handle(command, cancellationToken);

        _productRepositoryMock.Verify(r => r.FindByIdAsync(productId, cancellationToken), Times.Once);
        _productRepositoryMock.Verify(r => r.FindBySkuAsync(sku, cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(cancellationToken), Times.Once);
        Assert.Single(product.Variants);
        Assert.Equal(sku, product.Variants.First().Sku);
        Assert.Equal(price, product.Variants.First().Price);
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenProductNotFound()
    {
        var productId = new ProductId(Guid.NewGuid());
        var sku = Sku.Create("VAR002");
        var price = Money.Create(60, "USD");
        var selections = new Dictionary<ProductOptionId, OptionValueId>();
        var command = new AddProductVariantCommand(productId, sku, price, selections);
        var cancellationToken = CancellationToken.None;

        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, cancellationToken))
            .ReturnsAsync((Product?)null); // Product not found

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, cancellationToken)
        );
        Assert.Equal("Product not found.", exception.Message);
        _productRepositoryMock.Verify(r => r.FindByIdAsync(productId, cancellationToken), Times.Once);
        _productRepositoryMock.Verify(r => r.FindBySkuAsync(It.IsAny<Sku>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenSkuAlreadyExists()
    {
        var productId = new ProductId(Guid.NewGuid());
        var sku = Sku.Create("VAR003");
        var price = Money.Create(70, "USD");
        var selections = new Dictionary<ProductOptionId, OptionValueId>();
        var command = new AddProductVariantCommand(productId, sku, price, selections);
        var cancellationToken = CancellationToken.None;

        var product = Product.Create(productId, "Test Product", "Description");
        var existingProductWithSku = Product.Create(new ProductId(Guid.NewGuid()), "Another Product", "Description");

        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, cancellationToken))
            .ReturnsAsync(product);
        _productRepositoryMock
            .Setup(r => r.FindBySkuAsync(sku, cancellationToken))
            .ReturnsAsync(existingProductWithSku); // SKU already exists

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, cancellationToken)
        );
        Assert.Equal($"SKU {sku} already exists.", exception.Message);
        _productRepositoryMock.Verify(r => r.FindByIdAsync(productId, cancellationToken), Times.Once);
        _productRepositoryMock.Verify(r => r.FindBySkuAsync(sku, cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        Assert.Empty(product.Variants); // No variant should be added to the product
    }
}
