using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

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
    }
}
