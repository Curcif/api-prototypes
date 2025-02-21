using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for CreateSale operation
        /// </summary>
        public CreateSaleProfile()
        {
            CreateMap<CreateSaleRequest, CreateSaleCommand>();
            CreateMap<CreateSaleCommand, Sale>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => string.Join(", ", src.Items.Select(i => i.Product))))
                .ForMember(dest => dest.Quantities, opt => opt.MapFrom(src => src.Items.Sum(i => i.Quantity)))
                .ForMember(dest => dest.UnitPrices, opt => opt.MapFrom(src => src.Items.Average(i => i.UnitPrice)));

            CreateMap<Sale, CreateSaleResult>();
        }
    }
}
