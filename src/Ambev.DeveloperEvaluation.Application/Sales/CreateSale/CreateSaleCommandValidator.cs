using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
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
        public CreateSaleCommandValidator() {
            RuleFor(sale => sale.Branch).NotEmpty();
            RuleFor(sale => sale.Products).NotEmpty();
            RuleFor(sale => sale.Quantities).NotEmpty();
            RuleFor(sale => sale.TotalAmount).NotEmpty();
            RuleFor(sale => sale.Customer).NotEmpty();
        }
    }
}