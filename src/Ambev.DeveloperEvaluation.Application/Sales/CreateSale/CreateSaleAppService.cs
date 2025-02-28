using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleAppService : ICreateSaleAppService
    {
        private readonly IMapper _mapper;
        private readonly ISaleCreationService _saleCreationService;

        public CreateSaleAppService(IMapper mapper, ISaleCreationService saleCreationService)
        {
            _mapper = mapper;
            _saleCreationService = saleCreationService;
        }

        public async Task<Sale> CreateSaleAsync(CreateSaleCommand command, decimal totalAmount, CancellationToken cancellationToken)
        {
            var creationData = _mapper.Map<SaleCreationData>(command);
            return await _saleCreationService.CreateSaleAsync(creationData, totalAmount, cancellationToken);
        }
    }
}
