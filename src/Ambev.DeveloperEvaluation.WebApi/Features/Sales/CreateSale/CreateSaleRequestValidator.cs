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
        /// - Customer: Must not be null or empty
        /// - Products: Must not be null or empty (on Items)
        /// - Quantities: Must not be null or empty or zero (on Items)
        /// - Unitprices: Must not be null or empty or zero (on Items)
        /// </remarks>
        public CreateSaleRequestValidator()
        {
            RuleFor(request => request.Branch).NotEmpty();
            RuleFor(request => request.Customer).NotEmpty();
            RuleFor(request => request.Items)
                .NotEmpty().WithMessage("At least one item is required.")
                .Must(items => items.Count <= 25).WithMessage("A sale cannot have more than 25 items.");
        }
    }
}