using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public interface ISaleCreationService
    {
        Task<Sale> CreateSaleAsync(SaleCreationData creationData, decimal totalAmount, CancellationToken cancellationToken);
    }
}
