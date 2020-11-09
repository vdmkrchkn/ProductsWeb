using ProductsWebApi.Models.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsWebApi.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders();

        Task<Order> GetOrderById(long id);

        Task<long> Add(Order order);
    }
}
