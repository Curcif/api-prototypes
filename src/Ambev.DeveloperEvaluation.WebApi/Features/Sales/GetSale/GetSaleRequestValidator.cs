using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Validator for GetSaleRequest
    /// </summary>
    public class GetSaleRequestValidator : AbstractValidator<GetSaleRequest>
    {
        public GetSaleRequestValidator() {
            RuleFor(request => request.Id)
            .NotEmpty().WithMessage("Sale ID is required.")
            .GreaterThan(0).WithMessage("Sale ID must be greater than 0.");
        }
    }
}