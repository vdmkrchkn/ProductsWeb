using Microsoft.AspNetCore.Mvc;
using ProductsWebApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Products.Web.Core.Models;
using Products.Web.Core.Services;

namespace ProductsWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/order")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/order
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            return _orderService.GetOrders();
        }

        // GET: api/order/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var order = await _orderService.GetOrderById(id);

            return Ok(order);
        }
        
        // POST: api/order
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderId = await _orderService.Add(order);

            return Created($"api/order/{orderId}", null);
        }
        
        // PUT: api/order/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]Order value)
        {
            throw new System.NotImplementedException();
        }

        // DELETE: api/order/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
