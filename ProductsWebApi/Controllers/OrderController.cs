using Microsoft.AspNetCore.Mvc;
using ProductsWebApi.Models.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/order")]
    public class OrderController : Controller
    {
        static List<Order> orders = new List<Order>();
        static int orderIdCounter = -1;

        // GET: api/order
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            return orders;
        }

        // GET: api/order/5
        [HttpGet("{id}", Name = "Get")]
        public Order Get(int id)
        {
            return orders[id];
        }
        
        // POST: api/order
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            orders.Add(order);
            ++orderIdCounter;

            return Created($"api/order/{orderIdCounter}", order);
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
