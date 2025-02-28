using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    public class SaleCreationService : ISaleCreationService
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public SaleCreationService(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<Sale> CreateSaleAsync(SaleCreationData creationData, decimal totalAmount, CancellationToken cancellationToken)
        {
            var sale = _mapper.Map<Sale>(creationData);
            sale.Products = string.Join(", ", creationData.Items!.Select(i => i.Product));
            sale.Quantities = creationData.Items!.Sum(i => i.Quantity);
            sale.UnitPrices = creationData.Items!.Average(i => i.UnitPrice);
            sale.TotalAmount = totalAmount;

            return await _saleRepository.CreateAsync(sale, cancellationToken);
        }
    }
}
