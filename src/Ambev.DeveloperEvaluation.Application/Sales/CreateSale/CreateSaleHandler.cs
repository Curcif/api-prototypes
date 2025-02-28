using Ambev.DeveloperEvaluation.Application.Sales.Validation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Handler for processing CreateSaleCommand requests
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ISaleDiscountService _discountService;
        private readonly ICreateSaleCommandValidatorService _validatorService;
        private readonly ICreateSaleAppService _createSaleAppService;
        private readonly ILogger<CreateSaleHandler> _logger;
        /// <summary>
        /// Initializes a new instance of CreateSaleHandler
        /// </summary>
        /// <param name="saleRepository">The sale repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        /// <param name="discountService">The service for sale that performs discount calculations</param>
        /// <param name="validatorService">The service for sale that is responsible for validations</param>
        public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, ISaleDiscountService discountService, ICreateSaleCommandValidatorService validatorService, ICreateSaleAppService createSaleAppService, ILogger<CreateSaleHandler> logger)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _discountService = discountService;
            _validatorService = validatorService;
            _createSaleAppService = createSaleAppService;
            _logger = logger;
        }

        /// <summary>
        /// Handles the CreateSaleCommand request
        /// </summary>
        /// <param name="command">The CreateSale command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale details</returns>
        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling CreateSaleCommand with SaleId: {SaleId}", command.SaleId);

            //Validations
            //CreateSaleCommandValidatorService
            //CreateSaleCommandValidator
            var validationResult = await _validatorService.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
            {
                var errorMessages = string.Join(", ", validationResult.Errors.Select(e => e.Error));
                _logger.LogError("Validation failed: {Errors}", errorMessages);
                throw new ValidationException(errorMessages);
            }

            //Calcs
            decimal totalAmount = _discountService.CalculateTotalAmount(command.Items);

            //Mappings
            var sale = await _createSaleAppService.CreateSaleAsync(command, totalAmount, cancellationToken).ConfigureAwait(false);

            var result = _mapper.Map<CreateSaleResult>(sale);
            return result;
        }
    }
}