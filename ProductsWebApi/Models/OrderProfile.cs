using AutoMapper;
using ProductsWebApi.Models.Entities;
using ProductsWebApi.Models.Json;

namespace ProductsWebApi.Models
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
