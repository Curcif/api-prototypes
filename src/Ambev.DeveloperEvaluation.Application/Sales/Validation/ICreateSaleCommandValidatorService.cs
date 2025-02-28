using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Application.Sales.Validation
{
    public interface ICreateSaleCommandValidatorService
    {
        Task<ValidationResultDetail> ValidateAsync(CreateSaleCommand command, CancellationToken cancellationToken);
    }
}
