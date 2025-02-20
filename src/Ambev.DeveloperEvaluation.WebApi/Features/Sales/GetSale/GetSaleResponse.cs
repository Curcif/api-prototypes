namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    public class GetSaleResponse
    {
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
    }
}
