using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Validator for UpdateSaleCommand that defines validation rules for sale update command.
    /// </summary>
    public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
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
        public UpdateSaleCommandValidator()
        {
            RuleFor(sale => sale.SaleId).NotEmpty();
            RuleFor(sale => sale.TotalAmount).NotEmpty();
            RuleFor(sale => sale.Branch).NotEmpty();
            RuleFor(sale => sale.Customer).NotEmpty();
            RuleFor(sale => sale.Items).NotEmpty().WithMessage("At least one item is required.");
            RuleForEach(sale => sale.Items).SetValidator(new SaleItemDtoValidator());
        }
    }
}