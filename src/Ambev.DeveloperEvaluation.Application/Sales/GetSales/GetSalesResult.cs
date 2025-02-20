namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public class GetSalesResult
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Customer { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}
