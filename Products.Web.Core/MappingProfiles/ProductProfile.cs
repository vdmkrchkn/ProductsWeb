using System.Globalization;
using AutoMapper;
using Products.Web.Core.Models;
using Products.Web.Infrastructure.Entities;

namespace Products.Web.Core.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            var doubleFormat = CultureInfo.InvariantCulture;

            CreateMap<Product, ProductEntity>(MemberList.None)
                .ForMember(productEntity => productEntity.Price,
                    opt => opt.MapFrom(product => decimal.Parse(product.Price, doubleFormat)));
            CreateMap<ProductEntity, Product>(MemberList.None)
                .ForMember(product => product.Image, opt => opt.Ignore())
                .ForMember(product => product.Price,
                    opt => opt.MapFrom(productEntity => productEntity.Price.ToString(doubleFormat)));
        }
    }
}
