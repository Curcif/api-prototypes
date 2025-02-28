using Ambev.DeveloperEvaluation.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public class SaleDiscountService : ISaleDiscountService
    {
        /// <summary>
        /// Calculates the total amount of a sale, applying discounts based on item quantities.
        /// </summary>
        /// <param name="items">List of sale items.</param>
        /// <returns>The total sale amount after applying discounts.</returns>
        /// <exception cref="ValidationException">Thrown if the quantity of an item exceeds 20.</exception>
        public decimal CalculateTotalAmount(List<SaleItemDto> items)
        {
            decimal totalAmount = 0;
            foreach (var item in items)
            {
                // Validate item quantity
                if (item.Quantity > 20)
                    throw new ValidationException($"Quantity for product '{item.Product}' cannot exceed 20.");

                // Calculate discount based on quantity
                decimal discount = CalculateDiscount(item.Quantity);

                // Calculate total item amount with discount and add to the sale total
                totalAmount += item.Quantity * item.UnitPrice * (1 - discount);
            }
            return totalAmount;
        }

        /// <summary>
        /// Calculates the discount based on the item quantity.
        /// </summary>
        /// <param name="quantity">Item quantity.</param>
        /// <returns>The discount rate applied.</returns>
        private decimal CalculateDiscount(int quantity)
        {
            return quantity switch
            {
                >= 10 => 0.20m, // 20% discount for 10+ items
                >= 4 => 0.10m,   // 10% discount for 4+ items
                _ => 0m          // No discount for less than 4 items
            };
        }
    }
}
