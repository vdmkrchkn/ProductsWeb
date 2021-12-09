using System.Collections.Generic;
using System.Threading.Tasks;
using Products.Web.Core.Models;

namespace Products.Web.Core.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetOrders();

        Task<Order> GetOrderById(long id);

        Task<long> Add(Order order);
    }
}
