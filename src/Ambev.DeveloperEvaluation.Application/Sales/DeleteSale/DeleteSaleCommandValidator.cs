using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    /// <summary>
    /// Validator for DeleteSaleCommand
    /// </summary>
    internal class DeleteSaleCommandValidator : AbstractValidator<DeleteSaleCommand>
    {
        /// <summary>
        /// Initializes validation rules for DeleteSaleCommand
        /// </summary>
        public DeleteSaleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale ID is required");
        }
    }
}
