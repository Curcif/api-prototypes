using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public interface ICreateSaleAppService
    {
        Task<CreateSaleResult> CreateSaleAsync(CreateSaleCommand command, decimal totalAmount, CancellationToken cancellationToken);
    }
}
