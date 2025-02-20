using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public class GetSalesHandler : IRequestHandler<GetSalesCommand, List<GetSalesResult>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<List<GetSalesResult>> Handle(GetSalesCommand request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<List<GetSalesResult>>(sales);
        }
    }
}
