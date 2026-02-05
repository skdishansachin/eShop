using eShop.Applcation.Catalog.Commands.AddOptionValue;
using eShop.Applcation.SharedKernel;
using eShop.Domain.Catalog;
using eShop.Domain.SharedKernel.ValueObjects;
using Moq;

namespace eShop.Application.Tests.Catalog;

public class AddOptionValueHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AddOptionValueHandler _handler;

    public AddOptionValueHandlerTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new AddOptionValueHandler(_productRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldAddOptionValue_WhenProductAndOptionExist()
    {
        var productId = new ProductId(Guid.NewGuid());
        var productOptionId = new ProductOptionId(Guid.NewGuid());
        var optionName = OptionName.Create("Color");
        var optionValueName = OptionValueName.Create("Red");
        var command = new AddOptionValueCommand(productId, productOptionId, optionValueName);
        var cancellationToken = CancellationToken.None;

        var product = Product.Create(productId, "Test Product", "Description");
        product.AddOption(productOptionId, optionName); // Add the option first
        
        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, cancellationToken))
            .ReturnsAsync(product);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(cancellationToken)).ReturnsAsync(1);

        await _handler.Handle(command, cancellationToken);

        _productRepositoryMock.Verify(r => r.FindByIdAsync(productId, cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(cancellationToken), Times.Once);
        
        var addedOption = product.Options.FirstOrDefault(o => o.Id == productOptionId);
        Assert.NotNull(addedOption);
        Assert.Single(addedOption.Values);
        Assert.Equal(optionValueName, addedOption.Values.First().Name);
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidOperationException_WhenProductNotFound()
    {
        var productId = new ProductId(Guid.NewGuid());
        var productOptionId = new ProductOptionId(Guid.NewGuid());
        var optionValueName = OptionValueName.Create("Green");
        var command = new AddOptionValueCommand(productId, productOptionId, optionValueName);
        var cancellationToken = CancellationToken.None;

        _productRepositoryMock
            .Setup(r => r.FindByIdAsync(productId, cancellationToken))
            .ReturnsAsync((Product?)null);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _handler.Handle(command, cancellationToken)
        );
        Assert.Equal("Product not found.", exception.Message);
        _productRepositoryMock.Verify(r => r.FindByIdAsync(productId, cancellationToken), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}