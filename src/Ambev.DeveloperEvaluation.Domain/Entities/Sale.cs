using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        /// <summary>
        /// The unique identification (Id) for each sale
        /// </summary>
        public int? SaleId { get; set; }
        /// <summary>
        /// Represents the date and time when the sale was made
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// The Customer which made the sale
        /// </summary>
        public string Customer { get; set; } = string.Empty;
        /// <summary>
        /// Total sale amount
        /// </summary>
        public decimal? Total { get; set; }
        /// <summary>
        /// Where the sale was made
        /// </summary>
        public string Branch { get; set; } = string.Empty;
        /// <summary>
        /// Products contained in sale
        /// </summary>
        public string Products { get; set; } = string.Empty;
        /// <summary>
        /// Quantity of items in the cart
        /// </summary>
        public int? Quantities { get; set; }
        /// <summary>
        /// The unit price from the product
        /// </summary>
        public decimal? UnitPrices { get; set; }
        /// <summary>
        /// The discount value applied to the sale
        /// </summary>
        public decimal? Discounts { get; set; }
        /// <summary>
        /// The total amount for each item
        /// </summary>
        public decimal? TotalAmount { get; set; }
        /// <summary>
        /// If the order is/was cancelled
        /// </summary>
        public bool? IsCancelled { get; set; }
        /// <summary>
        /// When the sale was created
        /// </summary>
        public DateTime? SaleCreated { get; set; }
        /// <summary>
        /// When the sale was modified
        /// </summary>
        public DateTime? SaleModified { get; set; }
        /// <summary>
        /// When the sale was cancelled
        /// </summary>
        public DateTime? SaleCancelled { get; set; }
        /// <summary>
        /// Items cancelled
        /// </summary>
        public bool? ItemCancelled { get; set; }
    }
}