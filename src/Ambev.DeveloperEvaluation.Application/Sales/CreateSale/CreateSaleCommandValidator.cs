using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    ///// <summary>
    ///// Validator for CreateSaleCommand that defines validation rules for sale creation command.
    ///// </summary>
    //public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    //{
    //    //private readonly ISaleRepository _saleRepository;
    //    /// <summary>
    //    /// Initializes a new instance of the CreateSaleCommandValidator with defined validation rules.
    //    /// </summary>
    //    /// <remarks>
    //    /// Validation rules include:
    //    /// - Branch: Must not be null or empty
    //    /// - Products: Must not be null or empty
    //    /// - Quantities: Must not be null or empty or zero
    //    /// - TotalAmount: Must not be null or empty or zero
    //    /// - Customer: Must not be null or empty
    //    /// </remarks>
    //    public CreateSaleCommandValidator()
    //    {
    //        //_saleRepository = saleRepository;

    //        RuleFor(sale => sale.Items.Count).LessThanOrEqualTo(25)
    //            .WithMessage("A sale cannot have more than 25 items.");
    //        RuleFor(sale => sale.Branch).NotEmpty().WithMessage("Branch is required.");
    //        RuleFor(sale => sale.Customer).NotEmpty().WithMessage("Customer is required.");
    //        RuleFor(sale => sale.Items)
    //            .NotEmpty().WithMessage("At least one item is required.")
    //            .Must(items => items.Count <= 25).WithMessage("A sale cannot have more than 25 items.");
    //        //RuleFor(sale => sale.SaleId).MustAsync(async (saleId, cancellation) =>
    //        //{
    //        //    var existingSale = await _saleRepository.GetByIdAsync(saleId, cancellation);
    //        //    return existingSale == null;
    //        //}).WithMessage("Sale with the specified Id already exists.");
    //    }
    //}


    /// <summary>
    /// Validator for CreateSaleCommand that defines validation rules for sale creation command.
    /// </summary>
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
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
        public CreateSaleCommandValidator()
        {
            RuleFor(sale => sale.Branch).NotEmpty();
            RuleFor(sale => sale.Customer).NotEmpty();
            RuleFor(sale => sale.Items)
                .NotEmpty().WithMessage("At least one item is required.")
                .Must(items => items.Count <= 25).WithMessage("A sale cannot have more than 25 items.");
        }
    }
}