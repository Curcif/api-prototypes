using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Common.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.Validation
{
    public class CreateSaleCommandValidatorService : ICreateSaleCommandValidatorService
    {
        private readonly CreateSaleCommandValidator _validator;

        public CreateSaleCommandValidatorService(CreateSaleCommandValidator validator)
        {
            _validator = new CreateSaleCommandValidator();
        }

        public async Task<ValidationResultDetail> ValidateAsync(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(command, cancellationToken); // Chamada assíncrona
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => new ValidationErrorDetail
                {
                    Detail = o.PropertyName,
                    Error = o.ErrorMessage
                }).ToList()
            };
        }
    }
}
