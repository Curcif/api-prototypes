using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Profile for mapping between Sale entity and CreateSaleResponse
    /// </summary>
    public class CreateSaleProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for CreateSale operation
        /// </summary>
        public CreateSaleProfile()
        {
            CreateMap<CreateSaleCommand, Sale>()
             .ForMember(dest => dest.Products, opt => opt.MapFrom(src => string.Join(", ", src.Items.Select(i => i.Product))))
             .ForMember(dest => dest.Quantities, opt => opt.MapFrom(src => src.Items.Sum(i => i.Quantity)))
             .ForMember(dest => dest.UnitPrices, opt => opt.MapFrom(src => src.Items.Average(i => i.UnitPrice)));

            CreateMap<Sale, CreateSaleResult>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));
        }
    }
}