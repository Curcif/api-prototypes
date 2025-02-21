using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateSaleHandlerTests
    {
        private readonly Mock<ISaleRepository> _mockSaleRepository;
        private readonly Mock<IMapper> _mockMapper;

        public CreateSaleHandlerTests()
        {
            _mockSaleRepository = new Mock<ISaleRepository>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task CreateSale_ShouldAddSaleToDatabase()
        {
            var command = new CreateSaleCommand
            {
                Customer = "Customer 1",
                Total = 100.50m,
                Branch = "Branch A",
                Products = "Product 1",
                Quantities = 2,
                UnitPrices = 50.25m,
                Discounts = 10.00m,
                TotalAmount = 90.50m
            };

            var sale = new Sale
            {
                SaleId = 1,
                Customer = command.Customer,
                Total = command.Total,
                Branch = command.Branch,
                Products = command.Products,
                Quantities = command.Quantities,
                UnitPrices = command.UnitPrices,
                Discounts = command.Discounts,
                TotalAmount = command.TotalAmount
            };

            var createSaleResult = new CreateSaleResult { Id = (int)sale.SaleId };

            _mockSaleRepository
                .Setup(repo => repo.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sale);

            _mockMapper
                .Setup(mapper => mapper.Map<Sale>(command))
                .Returns(sale);

            _mockMapper
                .Setup(mapper => mapper.Map<CreateSaleResult>(sale))
                .Returns(createSaleResult);

            var handler = new CreateSaleHandler(_mockSaleRepository.Object, _mockMapper.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            _mockSaleRepository.Verify(repo => repo.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<Sale>(command), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<CreateSaleResult>(sale), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(sale.SaleId, result.Id);
        }

        [Fact]
        public async Task CreateSale_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            var command = new CreateSaleCommand
            {
                Customer = "Customer 1",
                Total = 100.50m,
                Branch = "Branch A",
                Products = "Product 1",
                Quantities = 2,
                UnitPrices = 50.25m,
                Discounts = 10.00m,
                TotalAmount = 90.50m
            };

            var sale = new Sale
            {
                SaleId = 1,
                Customer = command.Customer,
                Total = command.Total,
                Branch = command.Branch,
                Products = command.Products,
                Quantities = command.Quantities,
                UnitPrices = command.UnitPrices,
                Discounts = command.Discounts,
                TotalAmount = command.TotalAmount
            };

            _mockMapper
                .Setup(mapper => mapper.Map<Sale>(command))
                .Returns(sale);

            _mockSaleRepository
                .Setup(repo => repo.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error while trying to save to the repository."));

            var handler = new CreateSaleHandler(_mockSaleRepository.Object, _mockMapper.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
            Assert.Equal("Error while trying to save to the repository.", exception.Message);
        }

        [Fact]
        public async Task CreateSale_WithLessThan4Items_ShouldNotApplyDiscount()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                Customer = "Customer A",
                Total = 150.00m,
                Branch = "Branch A",
                Items = new List<DeveloperEvaluation.Application.Sales.CreateSale.SaleItemDto>
            {
                new DeveloperEvaluation.Application.Sales.CreateSale.SaleItemDto { Product = "Product A", Quantity = 3, UnitPrice = 50.00m }
            }
            };

            var sale = new Sale
            {
                SaleId = 1,
                Customer = command.Customer,
                Total = command.Total,
                Branch = command.Branch,
                Products = string.Join(", ", command.Items.Select(i => i.Product)),
                Quantities = command.Items.Sum(i => i.Quantity),
                UnitPrices = command.Items.Average(i => i.UnitPrice),
                Discounts = 0, // No discount
                TotalAmount = 150.00m
            };

            var createSaleResult = new CreateSaleResult { Id = (int)sale.SaleId };

            _mockSaleRepository
                .Setup(repo => repo.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sale);

            _mockMapper
                .Setup(mapper => mapper.Map<Sale>(command))
                .Returns(sale);

            _mockMapper
                .Setup(mapper => mapper.Map<CreateSaleResult>(sale))
                .Returns(createSaleResult);

            var handler = new CreateSaleHandler(_mockSaleRepository.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(150.00m, result.TotalAmount); // No discount applied
        }

        [Fact]
        public async Task CreateSale_With5Items_ShouldApply10PercentDiscount()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                Customer = "Customer B",
                Branch = "Branch B",
                Items = new List<DeveloperEvaluation.Application.Sales.CreateSale.SaleItemDto>
        {
            new DeveloperEvaluation.Application.Sales.CreateSale.SaleItemDto { Product = "Product B", Quantity = 5, UnitPrice = 50.00m }
        }
            };

            var sale = new Sale
            {
                SaleId = 2,
                Customer = command.Customer,
                Branch = command.Branch,
                Products = string.Join(", ", command.Items.Select(i => i.Product)),
                Quantities = command.Items.Sum(i => i.Quantity),
                UnitPrices = command.Items.Average(i => i.UnitPrice),
                Discounts = 10, // 10% discount
                TotalAmount = 225.00m // 250 - 10% = 225
            };

            var createSaleResult = new CreateSaleResult { Id = (int)sale.SaleId };

            _mockSaleRepository
                .Setup(repo => repo.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sale);

            _mockMapper
                .Setup(mapper => mapper.Map<Sale>(command))
                .Returns(sale);

            _mockMapper
                .Setup(mapper => mapper.Map<CreateSaleResult>(sale))
                .Returns(createSaleResult);

            var handler = new CreateSaleHandler(_mockSaleRepository.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(225.00m, result.TotalAmount); // 10% discount applied
        }

        [Fact]
        public async Task CreateSale_With12Items_ShouldApply20PercentDiscount()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                Customer = "Customer C",
                Total = 600.00m,
                Branch = "Branch C",
                Items = new List<DeveloperEvaluation.Application.Sales.CreateSale.SaleItemDto>
            {
                new DeveloperEvaluation.Application.Sales.CreateSale.SaleItemDto { Product = "Product C", Quantity = 12, UnitPrice = 50.00m }
            }
            };

            var sale = new Sale
            {
                SaleId = 3,
                Customer = command.Customer,
                Total = command.Total,
                Branch = command.Branch,
                Products = string.Join(", ", command.Items.Select(i => i.Product)),
                Quantities = command.Items.Sum(i => i.Quantity),
                UnitPrices = command.Items.Average(i => i.UnitPrice),
                Discounts = 20, // 20% discount
                TotalAmount = 480.00m // 600 - 20% = 480
            };

            var createSaleResult = new CreateSaleResult { Id = (int)sale.SaleId };

            _mockSaleRepository
                .Setup(repo => repo.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sale);

            _mockMapper
                .Setup(mapper => mapper.Map<Sale>(command))
                .Returns(sale);

            _mockMapper
                .Setup(mapper => mapper.Map<CreateSaleResult>(sale))
                .Returns(createSaleResult);

            var handler = new CreateSaleHandler(_mockSaleRepository.Object, _mockMapper.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(480.00m, result.TotalAmount); // 20% discount applied
        }

        [Fact]
        public async Task CreateSale_With25Items_ShouldThrowValidationException()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                Customer = "Customer D",
                Total = 1250.00m,
                Branch = "Branch D",
                Items = new List<DeveloperEvaluation.Application.Sales.CreateSale.SaleItemDto>
            {
                new DeveloperEvaluation.Application.Sales.CreateSale.SaleItemDto { Product = "Product D", Quantity = 25, UnitPrice = 50.00m }
            }
            };

            var handler = new CreateSaleHandler(_mockSaleRepository.Object, _mockMapper.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }

    }
}
