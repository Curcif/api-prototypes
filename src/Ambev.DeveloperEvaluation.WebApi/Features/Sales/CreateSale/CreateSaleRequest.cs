using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequest
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
        /// Gets or sets products, quantities eand unit prices
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
