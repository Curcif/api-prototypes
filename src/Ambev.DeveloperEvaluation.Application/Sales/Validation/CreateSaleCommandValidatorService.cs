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
        private readonly ILogger<CreateSaleCommandValidatorService> _logger;

        public CreateSaleCommandValidatorService(CreateSaleCommandValidator validator, ILogger<CreateSaleCommandValidatorService> logger)
        {
            _validator = new CreateSaleCommandValidator();
            _logger = logger;
        }

        public async Task<ValidationResultDetail> ValidateAsync(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(command, cancellationToken); // Chamada assíncrona

            if (!result.IsValid)
            {
                var errorMessages = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                _logger.LogError("Validation failed for SaleId {SaleId}: {Errors}", command.SaleId, errorMessages);
                throw new ValidationException(result.Errors);
            }

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
