using eShop.Applcation.Catalog.Commands.CreateProduct;
using eShop.Applcation.SharedKernel;
using eShop.Domain.Catalog;
using eShop.Domain.SharedKernel.ValueObjects;
using Moq;

namespace eShop.Application.Tests.Catalog;

public class CreateProductHandlerTests
{
    private readonly Mock<IProductRepository> _repository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly CreateProductHandler _handler;

    public CreateProductHandlerTests()
    {
        _repository = new Mock<IProductRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new CreateProductHandler(_repository.Object, _unitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateProductAndReturnProductId_WhenCommandIsValid()
    {
        var command = new CreateProductCommand("Test Product", "Description for test product");
        var cancellationToken = CancellationToken.None;

        _repository
            .Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var result = await _handler.Handle(command, cancellationToken);

        Assert.IsType<ProductId>(result);
        _repository.Verify(
            r =>
                r.AddAsync(
                    It.Is<Product>(p =>
                        p.Title == command.Title && p.Description == command.Description
                    ),
                    cancellationToken
                ),
            Times.Once
        );
        _unitOfWork.Verify(u => u.SaveChangesAsync(cancellationToken), Times.Once);
    }
}
