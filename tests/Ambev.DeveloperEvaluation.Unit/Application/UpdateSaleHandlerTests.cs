using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Moq;
using Xunit;

public class UpdateSaleHandlerTests
{
    private readonly Mock<ISaleRepository> _mockSaleRepository;
    private readonly Mock<IMapper> _mockMapper;

    public UpdateSaleHandlerTests()
    {
        _mockSaleRepository = new Mock<ISaleRepository>();
        _mockMapper = new Mock<IMapper>();
    }

    [Fact]
    public async Task UpdateSale_ShouldUpdateSaleInDatabase()
    {
        // Arrange
        var command = new UpdateSaleCommand
        {
            SaleId = 1,
            Customer = "Updated Customer",
            Total = 200.75m,
            Branch = "Updated Branch",
            Discounts = 15.00m,
            TotalAmount = 185.75m,
            IsCancelled = false,
            Date = DateTime.UtcNow,
            SaleCreated = DateTime.UtcNow.AddDays(-1),
            SaleModified = DateTime.UtcNow,
            SaleCancelled = null,
            ItemCancelled = false,
            Items = new List<SaleItemDto>
        {
            new SaleItemDto { Product = "Product 1", Quantity = 2, UnitPrice = 25.00m },
            new SaleItemDto { Product = "Product 2", Quantity = 3, UnitPrice = 30.00m }
        }
        };

        var existingSale = new Sale
        {
            SaleId = 1,
            Customer = "Old Customer",
            Total = 100.50m,
            Branch = "Old Branch",
            Products = "Old Product",
            Quantities = 2,
            UnitPrices = 50.25m,
            Discounts = 10.00m,
            TotalAmount = 90.50m,
            IsCancelled = false,
            Date = DateTime.UtcNow.AddDays(-2),
            SaleCreated = DateTime.UtcNow.AddDays(-2),
            SaleModified = DateTime.UtcNow.AddDays(-1),
            SaleCancelled = null,
            ItemCancelled = false
        };

        var updatedSale = new Sale
        {
            SaleId = 1,
            Customer = command.Customer,
            Total = command.Total,
            Branch = command.Branch,
            Products = string.Join(", ", command.Items.Select(i => i.Product)),
            Quantities = command.Items.Sum(i => i.Quantity),
            UnitPrices = command.Items.Average(i => i.UnitPrice),
            Discounts = command.Discounts,
            TotalAmount = command.TotalAmount,
            IsCancelled = command.IsCancelled,
            Date = command.Date,
            SaleCreated = command.SaleCreated,
            SaleModified = command.SaleModified,
            SaleCancelled = command.SaleCancelled,
            ItemCancelled = command.ItemCancelled
        };

        var updateSaleResult = new UpdateSaleResult
        {
            SaleId = (int)updatedSale.SaleId,
            Customer = updatedSale.Customer,
            Total = updatedSale.Total,
            Branch = updatedSale.Branch,
            Products = updatedSale.Products,
            Quantities = updatedSale.Quantities,
            UnitPrices = updatedSale.UnitPrices,
            Discounts = updatedSale.Discounts,
            TotalAmount = updatedSale.TotalAmount,
            IsCancelled = updatedSale.IsCancelled,
            Date = updatedSale.Date,
            SaleCreated = updatedSale.SaleCreated,
            SaleModified = updatedSale.SaleModified,
            SaleCancelled = updatedSale.SaleCancelled,
            ItemCancelled = updatedSale.ItemCancelled
        };

        // Configure the repository mock
        _mockSaleRepository
            .Setup(repo => repo.GetByIdAsync(command.SaleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingSale);

        _mockSaleRepository
            .Setup(repo => repo.UpdateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedSale);

        // Configure the AutoMapper mock
        _mockMapper
            .Setup(mapper => mapper.Map<UpdateSaleResult>(updatedSale))
            .Returns(updateSaleResult);

        var handler = new UpdateSaleHandler(_mockSaleRepository.Object, _mockMapper.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        _mockSaleRepository.Verify(repo => repo.GetByIdAsync(command.SaleId, It.IsAny<CancellationToken>()), Times.Once);
        _mockSaleRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<UpdateSaleResult>(updatedSale), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(updatedSale.SaleId, result.SaleId); // Use SaleId instead of Id
    }

    [Fact]
    public async Task UpdateSale_WithInvalidData_ShouldThrowValidationException()
    {
        // Arrange
        var command = new UpdateSaleCommand
        {
            SaleId = 1,
            Customer = "", // Invalid data (empty Customer)
            Total = 200.75m,
            Branch = "Updated Branch",
            Discounts = 15.00m,
            TotalAmount = 185.75m,
            IsCancelled = false,
            Date = DateTime.UtcNow,
            SaleCreated = DateTime.UtcNow.AddDays(-1),
            SaleModified = DateTime.UtcNow,
            SaleCancelled = null,
            ItemCancelled = false,
            Items = new List<SaleItemDto>
        {
            new SaleItemDto { Product = "Product 1", Quantity = 2, UnitPrice = 25.00m },
            new SaleItemDto { Product = "Product 2", Quantity = 3, UnitPrice = 30.00m }
        }
        };

        var handler = new UpdateSaleHandler(_mockSaleRepository.Object, _mockMapper.Object);

        await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateSale_WhenRepositoryThrowsException_ShouldPropagateException()
    {
        var command = new UpdateSaleCommand
        {
            SaleId = 1,
            Customer = "Updated Customer",
            Total = 200.75m,
            Branch = "Updated Branch",
            Discounts = 15.00m,
            TotalAmount = 185.75m,
            IsCancelled = false,
            Date = DateTime.UtcNow,
            SaleCreated = DateTime.UtcNow.AddDays(-1),
            SaleModified = DateTime.UtcNow,
            SaleCancelled = null,
            ItemCancelled = false,
            Items = new List<SaleItemDto>
        {
            new SaleItemDto { Product = "Product 1", Quantity = 2, UnitPrice = 25.00m },
            new SaleItemDto { Product = "Product 2", Quantity = 3, UnitPrice = 30.00m }
        }
        };

        var existingSale = new Sale
        {
            SaleId = 1,
            Customer = "Old Customer",
            Total = 100.50m,
            Branch = "Old Branch",
            Products = "Old Product",
            Quantities = 2,
            UnitPrices = 50.25m,
            Discounts = 10.00m,
            TotalAmount = 90.50m,
            IsCancelled = false,
            Date = DateTime.UtcNow.AddDays(-2),
            SaleCreated = DateTime.UtcNow.AddDays(-2),
            SaleModified = DateTime.UtcNow.AddDays(-1),
            SaleCancelled = null,
            ItemCancelled = false
        };

        _mockSaleRepository
            .Setup(repo => repo.GetByIdAsync(command.SaleId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingSale);

        _mockSaleRepository
            .Setup(repo => repo.UpdateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Error updating sale in the repository."));

        var handler = new UpdateSaleHandler(_mockSaleRepository.Object, _mockMapper.Object);

        var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        Assert.Equal("Error updating sale in the repository.", exception.Message);
    }
}