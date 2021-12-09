using AutoMapper;
using Products.Web.Core.Models;
using Products.Web.Infrastructure.Entities;

namespace Products.Web.Core.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderEntity>(MemberList.None);
            CreateMap<OrderEntity, Order>(MemberList.None);
        }
    }
}
