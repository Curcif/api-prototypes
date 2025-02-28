using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    /// <summary>
    /// Provides methods for generating test data for sales using the Bogus library.
    /// This class centralizes all test data generation to ensure consistency
    /// across test cases and provide both valid and invalid data scenarios.
    /// </summary>
    public static class SalesTestData
    {
        /// <summary>
        /// Configures the Faker to generate valid CreateSaleCommand entities.
        /// The generated commands will have valid:
        /// - Customer (random name)
        /// - Total (random decimal between 10 and 1000)
        /// - Branch (random company name)
        /// - Products (random product name)
        /// - Quantities (random number between 1 and 10)
        /// - UnitPrices (random decimal between 5 and 50)
        /// </summary>
        private static readonly Faker<CreateSaleCommand> CreateSaleCommandFaker = new Faker<CreateSaleCommand>()
            .RuleFor(s => s.Customer, f => f.Name.FullName())
            .RuleFor(s => s.Total, f => f.Random.Decimal(10, 1000))
            .RuleFor(s => s.Branch, f => f.Company.CompanyName())
            .RuleFor(s => s.Items, f => GenerateSaleItemsForCreate())
            .RuleFor(s => s.Discounts, f => f.Random.Decimal(0, 50))
            .RuleFor(s => s.TotalAmount, f => f.Random.Decimal(50, 1000))
            .RuleFor(s => s.IsCancelled, f => f.Random.Bool())
            .RuleFor(s => s.Date, f => f.Date.Past(1))
            .RuleFor(s => s.SaleCreated, f => f.Date.Past(1))
            .RuleFor(s => s.SaleModified, f => f.Date.Recent())
            .RuleFor(s => s.SaleCancelled, f => f.Random.Bool() ? f.Date.Past(1) : null)
            .RuleFor(s => s.ItemCancelled, f => f.Random.Bool());

        /// <summary>
        /// Configures the Faker to generate valid UpdateSaleCommand entities.
        /// The generated commands will have valid:
        /// - SaleId (random number between 1 and 100)
        /// - Customer (random name)
        /// - Total (random decimal between 10 and 1000)
        /// - Branch (random company name)
        /// - Items (list of SaleItemDto with random products, quantities, and unit prices)
        /// - Discounts (random decimal between 0 and 50)
        /// - IsCancelled (random boolean)
        /// - Dates (random dates within the last year)
        /// </summary>
        private static readonly Faker<UpdateSaleCommand> UpdateSaleCommandFaker = new Faker<UpdateSaleCommand>()
                 .RuleFor(s => s.SaleId, f => f.Random.Int(1, 100))
                 .RuleFor(s => s.Customer, f => f.Name.FullName())
                 .RuleFor(s => s.Total, f => f.Random.Decimal(10, 1000))
                 .RuleFor(s => s.Branch, f => f.Company.CompanyName())
                 .RuleFor(s => s.Discounts, f => f.Random.Decimal(0, 50))
                 .RuleFor(s => s.IsCancelled, f => f.Random.Bool())
                 .RuleFor(s => s.Date, f => f.Date.Past(1))
                 .RuleFor(s => s.SaleCreated, f => f.Date.Past(1))
                 .RuleFor(s => s.SaleModified, f => f.Date.Recent())
                 .RuleFor(s => s.SaleCancelled, f => f.Random.Bool() ? f.Date.Past(1) : null)
                 .RuleFor(s => s.ItemCancelled, f => f.Random.Bool())
                 .RuleFor(s => s.Items, f => GenerateSaleItemsForUpdate());


        /// <summary>
        /// Generates a list of SaleItemDto with random data.
        /// </summary>
        /// <returns>A list of SaleItemDto with random products, quantities, and unit prices.</returns>
        private static List<SaleItemDto> GenerateSaleItemsForCreate()
        {
            var faker = new Faker();
            return new Faker<SaleItemDto>()
                .RuleFor(i => i.Product, f => f.Commerce.ProductName())
                .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(5, 50))
                .Generate(faker.Random.Int(1, 5));
        }


        /// <summary>
        /// Generates a list of SaleItemDto with random data.
        /// </summary>
        /// <returns>A list of SaleItemDto with random products, quantities, and unit prices.</returns>
        private static List<SaleItemDto> GenerateSaleItemsForUpdate()
        {
            var faker = new Faker();
            return new Faker<SaleItemDto>()
                .RuleFor(i => i.Product, f => f.Commerce.ProductName())
                .RuleFor(i => i.Quantity, f => f.Random.Int(1, 10))
                .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(5, 50))
                .Generate(faker.Random.Int(1, 5));
        }

        /// <summary>
        /// Generates a valid CreateSaleCommand with randomized data.
        /// </summary>
        /// <returns>A valid CreateSaleCommand with randomly generated data.</returns>
        public static CreateSaleCommand GenerateValidCreateSaleCommand()
        {
            return CreateSaleCommandFaker.Generate();
        }

        /// <summary>
        /// Generates a valid UpdateSaleCommand with randomized data.
        /// </summary>
        /// <returns>A valid UpdateSaleCommand with randomly generated data.</returns>
        public static UpdateSaleCommand GenerateValidUpdateSaleCommand()
        {
            return UpdateSaleCommandFaker.Generate();
        }
    }
}
