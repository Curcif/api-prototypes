using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.CreateUser;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{/// <summary>
/// Validator for CreateSaleRequest that defines validation rules for sale creation.
/// </summary>
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        /// <summary>
        /// Initializes a new instance of the CreateSaleCommandValidator with defined validation rules.
        /// </summary>
        /// <remarks>
        /// Validation rules include:
        /// - Branch: Must not be null or empty
        /// - Products: Must not be null or empty
        /// - Quantities: Must not be null or empty or zero
        /// - TotalAmount: Must not be null or empty or zero
        /// - Customer: Must not be null or empty
        /// </remarks>
        public CreateSaleRequestValidator() {
            RuleFor(sale => sale.Branch).NotEmpty();
            RuleFor(sale => sale.Products).NotEmpty();
            RuleFor(sale => sale.Quantities).NotEmpty();
            RuleFor(sale => sale.TotalAmount).NotEmpty();
            RuleFor(sale => sale.Customer).NotEmpty();
        }
    }
}