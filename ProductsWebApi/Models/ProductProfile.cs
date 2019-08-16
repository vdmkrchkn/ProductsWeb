using AutoMapper;
using ProductsWebApi.Models.Entities;
using ProductsWebApi.Models.Views;

namespace ProductsWebApi.Models
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductEntity>(MemberList.None);
            CreateMap<ProductEntity, Product>(MemberList.None)
                .ForMember(x => x.Image, opt => opt.Ignore());
        }
    }
}
