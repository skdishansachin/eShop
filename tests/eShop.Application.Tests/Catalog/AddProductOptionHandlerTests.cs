using eShop.Applcation.Catalog.Commands.AddProductOption;
using eShop.Applcation.SharedKernel;
using eShop.Domain.Catalog;
using eShop.Domain.SharedKernel.ValueObjects;
using Moq;

namespace eShop.Application.Tests.Catalog;

public class AddProductOptionHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AddProductOptionHandler _handler;

    public AddProductOptionHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new AddProductOptionHandler(_productRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddProductOption_WhenProductExists()
    {
        var productId = new ProductId(Guid.NewGuid());
        var productOptionId = new ProductOptionId(Guid.NewGuid());
        var optionName = OptionName.Create("Color");
        var command = new AddProductOptionCommand(productId, productOptionId, optionName);
        var cancellationToken = CancellationToken.None;

        var product = Product.Create(productId, "Test Product", "Description");
        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, cancellationToken))
            .ReturnsAsync(product);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(cancellationToken)).ReturnsAsync(1);

        await _handler.Handle(command, cancellationToken);

        _productRepositoryMock.Verify(r => r.FindByIdAsync(productId, cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(cancellationToken), Times.Once);
        Assert.Single(product.Options);
        Assert.Equal(productOptionId, product.Options.First().Id);
        Assert.Equal(optionName, product.Options.First().Name);
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenProductNotFound()
    {
        var productId = new ProductId(Guid.NewGuid());
        var productOptionId = new ProductOptionId(Guid.NewGuid());
        var optionName = OptionName.Create("Size");
        var command = new AddProductOptionCommand(productId, productOptionId, optionName);
        var cancellationToken = CancellationToken.None;

        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, cancellationToken))
            .ReturnsAsync((Product?)null);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, cancellationToken)
        );
        Assert.Equal("Product not found", exception.Message);
        _productRepositoryMock.Verify(r => r.FindByIdAsync(productId, cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
