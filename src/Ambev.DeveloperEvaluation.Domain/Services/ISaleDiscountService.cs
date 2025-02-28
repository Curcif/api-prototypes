using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    /// <summary>
    /// An interface for the concrete SaleDiscountService
    /// </summary>
    public interface ISaleDiscountService
    {
        decimal CalculateTotalAmount(List<SaleItemDto> items);
    }
}
