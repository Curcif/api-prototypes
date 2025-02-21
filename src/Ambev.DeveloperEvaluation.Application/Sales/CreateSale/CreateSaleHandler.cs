using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Handler for processing CreateSaleCommand requests
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of CreateSaleHandler
        /// </summary>
        /// <param name="saleRepository">The sale repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="validator">The validator for CreateSaleCommand</param>
        public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the CreateSaleCommand request
        /// </summary>
        /// <param name="command">The CreateSale command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale details</returns>
        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);



            var existingSale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
            if (existingSale != null)
                throw new InvalidOperationException($"Sale with Id {command.SaleId} already exists");

            decimal totalAmount = 0;
            foreach (var item in command.Items)
            {
                if (item.Quantity > 20)
                {
                    throw new ValidationException($"Quantity for product '{item.Product}' cannot exceed 20.");
                }

                decimal discount = item.Quantity switch
                {
                    >= 10 => 0.20m, // 20% discount for 10+ items
                    >= 4 => 0.10m,   // 10% discount for 4+ items
                    _ => 0m          // No discount for less than 4 items
                };

                totalAmount += item.Quantity * item.UnitPrice * (1 - discount);
            }

            var sale = _mapper.Map<Sale>(command);

            sale.Products = string.Join(", ", command.Items.Select(i => i.Product));
            sale.Quantities = command.Items.Sum(i => i.Quantity);
            sale.UnitPrices = command.Items.Average(i => i.UnitPrice);

            var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

            var result = _mapper.Map<CreateSaleResult>(createdSale);
            return result;
        }
    }
}