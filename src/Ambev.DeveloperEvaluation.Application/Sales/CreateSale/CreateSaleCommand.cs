using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Command for creating a new sale.
    /// </summary>
    /// <remarks>
    /// This command is used to capture the required data for creating a sale, 
    /// including Sale, items, quantity, discount, among others. 
    /// It implements <see cref="IRequest{TResponse}"/> to initiate the request 
    /// that returns a <see cref="CreateSaleResult"/>.
    /// 
    /// The data provided in this command is validated using the 
    /// <see cref="CreateSaleCommandValidator"/> which extends 
    /// <see cref="AbstractValidator{T}"/> to ensure that the fields are correctly 
    /// populated and follow the required rules.
    /// </remarks>
    public class CreateSaleCommand : IRequest<CreateSaleResult>
    {
        /// <summary>
        /// Gets or sets the sale id.
        /// </summary>
        public int? SaleId { get; set; }
        /// <summary>
        /// Gets or sets the sale's date
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// Gets or sets the Customer
        /// </summary>
        public string Customer { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the total sale amount
        /// </summary>
        public decimal? Total { get; set; }
        /// <summary>
        /// Gets or sets the branch
        /// </summary>
        public string Branch { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the list of items in the sale.
        /// </summary>
        public List<SaleItemDto> Items { get; set; } = new();
        /// <summary>
        /// Gets or sets products
        /// </summary>
        public string Products { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets quantity of items in the cart
        /// </summary>
        public int? Quantities { get; set; }
        /// <summary>
        /// Gets or sets the unit price from the product
        /// </summary>
        public decimal? UnitPrices { get; set; }
        /// <summary>
        /// Gets or sets the discount value applied to the sale
        /// </summary>
        public decimal? Discounts { get; set; }
        /// <summary>
        /// Gets or sets the total amount for each item
        /// </summary>
        public decimal? TotalAmount { get; set; }
        /// <summary>
        /// Gets or sets the order status
        /// </summary>
        public bool? IsCancelled { get; set; }
        /// <summary>
        /// Gets or sets the creation date
        /// </summary>
        public DateTime? SaleCreated { get; set; }
        /// <summary>
        /// Gets or sets the modify date
        /// </summary>
        public DateTime? SaleModified { get; set; }
        /// <summary>
        /// Gets or sets the cancellation date
        /// </summary>
        public DateTime? SaleCancelled { get; set; }
        /// <summary>
        /// Gets or sets items cancelled
        /// </summary>
        public bool? ItemCancelled { get; set; }

        public ValidationResultDetail Validate()
        {
            var validator = new CreateSaleCommandValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
    /// <summary>
    /// Represents an item in the sale.
    /// </summary>
    public class SaleItemDto
    {
        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string Product { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the quantity of the product.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
