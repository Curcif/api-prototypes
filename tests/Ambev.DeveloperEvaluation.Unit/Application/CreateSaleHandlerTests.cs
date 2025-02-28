using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class CreateSaleHandlerTests
    {
        private readonly Mock<ILogger<CreateSaleHandler>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ISaleRepository> _mockSaleRepository;
        private ICreateSaleCommandValidatorService _validatorService;
        private readonly CreateSaleCommandValidator _validator;
        private readonly ISaleDiscountService _discountService;
        private readonly ISaleCreationService _saleCreationService;
        private readonly ICreateSaleAppService _mockCreateSaleAppService;

        public CreateSaleHandlerTests()
        {
            _mockLogger = new Mock<ILogger<CreateSaleHandler>>();
            _mockMapper = new Mock<IMapper>();
            _mockSaleRepository = new Mock<ISaleRepository>();
            _validator = new CreateSaleCommandValidator();
            _validatorService = new CreateSaleCommandValidatorService(_validator);
            _discountService = new SaleDiscountService();
            _saleCreationService = new SaleCreationService(_mockSaleRepository.Object, _mockMapper.Object);
            _mockCreateSaleAppService = new CreateSaleAppService(_mockMapper.Object, _saleCreationService);

        }

        [Fact]
        public async Task CreateSale_ShouldAddSaleToDatabase()
        {
            var command = new CreateSaleCommand
            {
                Customer = "Customer 1",
                Total = 100.50m,
                Branch = "Branch A",
                Items = new List<SaleItemDto>
       {
            new SaleItemDto { Product = "Product 1", Quantity = 2, UnitPrice = 50.25m }
       },
                Discounts = 10.00m,
                TotalAmount = 90.50m
            };

            var saleCreationData = new SaleCreationData
            {
                SaleId = 1,
                Customer = command.Customer,
                Total = command.Total,
                Branch = command.Branch,
                Items = new List<SaleItemDto>(),
                Discounts = command.Discounts,
                TotalAmount = 100.50m
            };

            if (command?.Items.Count > 0)
            {
                saleCreationData.Items.AddRange(command?.Items!);
            }

            var sale = new Sale
            {
                SaleId = 1,
                Customer = command?.Customer!,
                Total = command!.Total,
                Branch = command!.Branch,
                Products = string.Join(", ", command.Items.Select(i => i.Product)),
                Quantities = command.Items.Sum(i => i.Quantity),
                UnitPrices = command.Items.Average(i => i.UnitPrice),
                Discounts = command.Discounts,
                TotalAmount = 100.50m
            };

            var createSaleResult = new CreateSaleResult { Id = (int)sale.SaleId, TotalAmount = sale.TotalAmount };

            _mockSaleRepository
                .Setup(repo => repo.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sale);

            _mockMapper
                .Setup(mapper => mapper.Map<SaleCreationData>(command))
                .Returns(saleCreationData);

            _mockMapper
                .Setup(mapper => mapper.Map<Sale>(It.IsAny<SaleCreationData>()))
                .Returns(sale);


            _mockMapper
                .Setup(mapper => mapper.Map<CreateSaleResult>(sale))
                .Returns(createSaleResult);

            _validatorService = new CreateSaleCommandValidatorService(_validator);

            var handler = new CreateSaleHandler(
                _mockSaleRepository.Object,
                _mockMapper.Object,
                new SaleDiscountService(),
                _validatorService,
                _mockCreateSaleAppService,
                _mockLogger.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            _mockSaleRepository.Verify(repo => repo.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<Sale>(It.IsAny<SaleCreationData>()), Times.Once);

            Assert.NotNull(result);
            Assert.Equal(sale.SaleId, result.Id);
            Assert.Equal(100.50m, result.TotalAmount);
        }

        [Fact]
        public async Task CreateSale_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            var command = new CreateSaleCommand
            {
                Customer = "Customer 1",
                Total = 100.50m,
                Branch = "Branch A",
                Items = new List<SaleItemDto>
                {
                    new SaleItemDto { Product = "Product 1", Quantity = 2, UnitPrice = 50.25m }
                },
                Discounts = 10.00m,
                TotalAmount = 90.50m
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
                Discounts = command.Discounts,
                TotalAmount = command.TotalAmount
            };

            var saleCreationData = new SaleCreationData
            {
                SaleId = 1,
                Customer = command.Customer,
                Total = command.Total,
                Branch = command.Branch,
                Items = new List<SaleItemDto>(),
                Discounts = command.Discounts,
                TotalAmount = 100.50m
            };

            if (command?.Items.Count > 0)
            {
                saleCreationData.Items.AddRange(command?.Items!);
            }

            var createSaleResult = new CreateSaleResult { Id = (int)sale.SaleId, TotalAmount = sale.TotalAmount };

            _mockMapper
                .Setup(mapper => mapper.Map<SaleCreationData>(command))
                .Returns(saleCreationData);

            _mockMapper
                .Setup(mapper => mapper.Map<Sale>(It.IsAny<SaleCreationData>()))
                .Returns(sale);


            _mockMapper
                .Setup(mapper => mapper.Map<CreateSaleResult>(sale))
                .Returns(createSaleResult);

            _mockSaleRepository
                .Setup(repo => repo.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Error while trying to save to the repository."));

            _validatorService = new CreateSaleCommandValidatorService(_validator);

            var handler = new CreateSaleHandler(
                _mockSaleRepository.Object,
                _mockMapper.Object,
                new SaleDiscountService(),
                _validatorService,
                _mockCreateSaleAppService,
                _mockLogger.Object);

            var exception = await Assert.ThrowsAsync<Exception>(() => handler.Handle(command!, CancellationToken.None));
            Assert.Equal("Error while trying to save to the repository.", exception.Message);
        }

        [Fact]
        public async Task CreateSale_WithLessThan4Items_ShouldNotApplyDiscount()
        {
            var command = new CreateSaleCommand
            {
                Customer = "Customer A",
                Total = 150.00m,
                Branch = "Branch A",
                Items = new List<SaleItemDto>
        {
            new SaleItemDto { Product = "Product A", Quantity = 3, UnitPrice = 50.00m }
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

            var saleCreationData = new SaleCreationData
            {
                SaleId = 1,
                Customer = command.Customer,
                Total = command.Total,
                Branch = command.Branch,
                Items = new List<SaleItemDto>(),
                Discounts = command.Discounts,
                TotalAmount = 100.50m
            };

            if (command?.Items.Count > 0)
            {
                saleCreationData.Items.AddRange(command?.Items!);
            }

            var createSaleResult = new CreateSaleResult { Id = (int)sale.SaleId, TotalAmount = sale.TotalAmount };

            _mockMapper
                .Setup(mapper => mapper.Map<SaleCreationData>(command))
                .Returns(saleCreationData);

            _mockMapper
                .Setup(mapper => mapper.Map<Sale>(It.IsAny<SaleCreationData>()))
                .Returns(sale);


            _mockMapper
                .Setup(mapper => mapper.Map<CreateSaleResult>(sale))
                .Returns(createSaleResult);

            _mockSaleRepository
                .Setup(repo => repo.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sale);

            var handler = new CreateSaleHandler(
                _mockSaleRepository.Object,
                _mockMapper.Object,
                new SaleDiscountService(),
                _validatorService,
                _mockCreateSaleAppService,
                _mockLogger.Object);

            var result = await handler.Handle(command!, CancellationToken.None);

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
                Items = new List<SaleItemDto>
        {
            new SaleItemDto { Product = "Product B", Quantity = 5, UnitPrice = 50.00m }
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

            var saleCreationData = new SaleCreationData
            {
                SaleId = 1,
                Customer = command.Customer,
                Total = command.Total,
                Branch = command.Branch,
                Items = new List<SaleItemDto>(),
                Discounts = 10,
                TotalAmount = 225.00m
            };

            if (command?.Items.Count > 0)
            {
                saleCreationData.Items.AddRange(command?.Items!);
            }

            var createSaleResult = new CreateSaleResult { Id = (int)sale.SaleId, TotalAmount = sale.TotalAmount };

            _mockMapper
                .Setup(mapper => mapper.Map<SaleCreationData>(command))
                .Returns(saleCreationData);

            _mockMapper
                .Setup(mapper => mapper.Map<Sale>(It.IsAny<SaleCreationData>()))
                .Returns(sale);

            _mockMapper
                .Setup(mapper => mapper.Map<CreateSaleResult>(sale))
                .Returns(createSaleResult);

            _mockSaleRepository
                .Setup(repo => repo.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sale);

            var handler = new CreateSaleHandler(
                _mockSaleRepository.Object,
                _mockMapper.Object,
                new SaleDiscountService(),
                _validatorService,
                _mockCreateSaleAppService,
                _mockLogger.Object);

            // Act
            var result = await handler.Handle(command!, CancellationToken.None);

            // Assert
            Assert.Equal(225.00m, result.TotalAmount); // 10% discount applied
        }

        [Fact]
        public async Task CreateSale_With12Items_ShouldApply20PercentDiscount()
        {
            var command = new CreateSaleCommand
            {
                Customer = "Customer C",
                Total = 600.00m,
                Branch = "Branch C",
                Items = new List<SaleItemDto>
        {
            new SaleItemDto { Product = "Product C", Quantity = 12, UnitPrice = 50.00m }
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

            var saleCreationData = new SaleCreationData
            {
                SaleId = 1,
                Customer = command.Customer,
                Total = command.Total,
                Branch = command.Branch,
                Items = new List<SaleItemDto>(),
                Discounts = command.Discounts,
                TotalAmount = 100.50m
            };

            if (command?.Items.Count > 0)
            {
                saleCreationData.Items.AddRange(command?.Items!);
            }

            var createSaleResult = new CreateSaleResult { Id = (int)sale.SaleId, TotalAmount = sale.TotalAmount };

            _mockMapper
                .Setup(mapper => mapper.Map<SaleCreationData>(command))
                .Returns(saleCreationData);

            _mockMapper
                .Setup(mapper => mapper.Map<Sale>(It.IsAny<SaleCreationData>()))
                .Returns(sale);


            _mockMapper
                .Setup(mapper => mapper.Map<CreateSaleResult>(sale))
                .Returns(createSaleResult);

            _mockSaleRepository
                .Setup(repo => repo.CreateAsync(It.IsAny<Sale>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(sale);

            var handler = new CreateSaleHandler(
                _mockSaleRepository.Object,
                _mockMapper.Object,
                new SaleDiscountService(),
                _validatorService,
                _mockCreateSaleAppService,
                _mockLogger.Object);

            var result = await handler.Handle(command!, CancellationToken.None);

            Assert.Equal(480.00m, result.TotalAmount); // 20% discount applied
        }

        [Fact]
        public async Task CreateSale_With25Items_ShouldThrowValidationException()
        {
            var command = new CreateSaleCommand
            {
                Customer = "Customer D",
                Total = 1250.00m,
                Branch = "Branch D",
                Items = new List<SaleItemDto>()
            };

            for (int i = 0; i < 26; i++)
            {
                command.Items.Add(new SaleItemDto { Product = $"Product {i}", Quantity = 1, UnitPrice = 50.00m });
            }

            var handler = new CreateSaleHandler(
                _mockSaleRepository.Object,
                _mockMapper.Object,
                new SaleDiscountService(),
                _validatorService,
                _mockCreateSaleAppService,
                _mockLogger.Object);

            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task CreateSale_WithNoItems_ShouldThrowValidationException()
        {
            var command = new CreateSaleCommand
            {
                Customer = "Customer E",
                Total = 100.00m,
                Branch = "Branch E",
                Items = new List<SaleItemDto>() // Lista vazia
            };

            var handler = new CreateSaleHandler(
                _mockSaleRepository.Object,
                _mockMapper.Object,
                new SaleDiscountService(),
                _validatorService,
                _mockCreateSaleAppService,
                _mockLogger.Object);

            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}