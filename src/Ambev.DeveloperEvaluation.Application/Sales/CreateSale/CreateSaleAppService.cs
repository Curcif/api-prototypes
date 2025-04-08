using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleAppService : ICreateSaleAppService
    {
        private readonly ILogger<CreateSaleAppService> _logger;
        private readonly IMapper _mapper;
        private readonly ISaleCreationService _saleCreationService;

        public CreateSaleAppService(ILogger<CreateSaleAppService> logger, IMapper mapper, ISaleCreationService saleCreationService)
        {
            _logger = logger;
            _mapper = mapper;
            _saleCreationService = saleCreationService;
        }

        public async Task<CreateSaleResult> CreateSaleAsync(CreateSaleCommand command, decimal totalAmount, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating Sale");

            var creationData = _mapper.Map<SaleCreationData>(command);
            var sale = await _saleCreationService.CreateSaleAsync(creationData, totalAmount, cancellationToken);
            
            _logger.LogInformation("Sale created successfully");

            return _mapper.Map<CreateSaleResult>(sale);
        }
    }
}
