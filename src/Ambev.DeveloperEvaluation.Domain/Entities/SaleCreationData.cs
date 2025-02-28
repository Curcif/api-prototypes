namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleCreationData
    {
        public int? SaleId { get; set; }
        public DateTime? Date { get; set; }
        public string? Customer { get; set; }
        public decimal? Total { get; set; }
        public string? Branch { get; set; }
        public List<SaleItemDto>? Items { get; set; }
        public decimal? Discounts { get; set; }
        public decimal? TotalAmount { get; set; }
        public bool? IsCancelled { get; set; }
        public DateTime? SaleCreated { get; set; }
        public DateTime? SaleModified { get; set; }
        public DateTime? SaleCancelled { get; set; }
        public bool? ItemCancelled { get; set; }
    }
}
