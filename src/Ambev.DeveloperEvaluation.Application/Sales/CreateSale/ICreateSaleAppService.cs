using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public interface ICreateSaleAppService
    {
        Task<Sale> CreateSaleAsync(CreateSaleCommand command, decimal totalAmount, CancellationToken cancellationToken);
    }
}
