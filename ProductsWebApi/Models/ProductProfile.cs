using AutoMapper;
using ProductsWebApi.Models.Entities;
using ProductsWebApi.Models.Json;
using System.Globalization;

namespace ProductsWebApi.Models
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            var doubleFormat = CultureInfo.InvariantCulture;

            CreateMap<Product, ProductEntity>(MemberList.None)
                .ForMember(productEntity => productEntity.Price,
                    opt => opt.MapFrom(product => double.Parse(product.Price, doubleFormat)));
            CreateMap<ProductEntity, Product>(MemberList.None)
                .ForMember(product => product.Image, opt => opt.Ignore())
                .ForMember(product => product.Price,
                    opt => opt.MapFrom(productEntity => productEntity.Price.ToString(doubleFormat)));

            CreateMap<Order, OrderEntity>(MemberList.None);
            CreateMap<OrderEntity, Order>(MemberList.None);
        }
    }
}
