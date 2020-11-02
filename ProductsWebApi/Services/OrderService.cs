using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ProductsWebApi.Models;
using ProductsWebApi.Models.Entities;
using ProductsWebApi.Models.Json;

namespace ProductsWebApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IRepository<OrderEntity> orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<long> Add(Order order)
        {
            var orderEntity = _mapper.Map<OrderEntity>(order);
            var orderId = await _orderRepository.Create(orderEntity);
            return orderId;
        }

        public async Task<Order> GetOrderById(long id)
        {
            var orderEntity = await _orderRepository.GetItemById(id);

            var order = _mapper.Map<Order>(orderEntity);

            return order;
        }

        public IEnumerable<Order> GetOrders()
        {
            var orderEntities = _orderRepository.GetItemList();

            var orders = _mapper.Map<IEnumerable<Order>>(orderEntities);

            return orders;
        }
    }
}
