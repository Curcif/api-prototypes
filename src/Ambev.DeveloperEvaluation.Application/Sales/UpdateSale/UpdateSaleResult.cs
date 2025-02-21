using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleResult
    {
        /// <summary>
        /// The Sale ID
        /// </summary>
        public int SaleId { get; set; }

        /// <summary>
        /// Sale's date
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// Customer's name
        /// </summary>
        public string Customer { get; set; } = string.Empty;
        /// <summary>
        /// Tthe total sale amount
        /// </summary>
        public decimal? Total { get; set; }
        /// <summary>
        /// The store where the buy was made
        /// </summary>
        public string Branch { get; set; } = string.Empty;
        /// <summary>
        /// The products bought
        /// </summary>
        public string Products { get; set; } = string.Empty;
        /// <summary>
        /// The quantity of items in the bought
        /// </summary>
        public int? Quantities { get; set; }
        /// <summary>
        /// The unit price from the product
        /// </summary>
        public decimal? UnitPrices { get; set; }
        /// <summary>
        /// Discount value applied to the sale
        /// </summary>
        public decimal? Discounts { get; set; }
        /// <summary>
        /// Total amount for each item
        /// </summary>
        public decimal? TotalAmount { get; set; }
        /// <summary>
        /// Items cancelled
        /// </summary>
        public bool? ItemCancelled { get; set; }
        /// <summary>
        /// The order status
        /// </summary>
        public bool? IsCancelled { get; set; }
        /// <summary>
        /// The creation date
        /// </summary>
        public DateTime? SaleCreated { get; set; }
        /// <summary>
        /// The modify date
        /// </summary>
        public DateTime? SaleModified { get; set; }
        /// <summary>
        /// The cancellation date
        /// </summary>
        public DateTime? SaleCancelled { get; set; }
    }
}
