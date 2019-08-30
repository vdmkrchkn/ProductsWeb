using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsWebApi.Models.Json;
using ProductsWebApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/product
        [HttpGet]
        public IEnumerable<ProductBase> GetProducts(string name, double? priceMin, double? priceMax)
        {
            return _productService.GetProducts(name, priceMin, priceMax);
        }

        // GET: api/product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productService.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);            
        }

        // PUT: api/product/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutProductEntity([FromRoute] long id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            product.Id = id;

            bool isSuccess = await _productService.Edit(product);

            if (isSuccess)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT: api/product/image/5
        [HttpPut("image/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutProductEntityWithImage(
            [FromRoute] long id,
            IFormFile image,
            Product product)
        {
            product.Image = image;

            return await PutProductEntity(id, product);
        }

        // POST: api/product
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PostProductEntity([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productService.Add(product);

            return CreatedAtAction("PostProductEntity", new { id = product.Id }, product);
        }

        // POST: api/product/image
        [HttpPost("image")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PostProductEntityWithImage(IFormFile image, Product product)
        {
            product.Image = image;

            return await PostProductEntity(product);
        }

        // DELETE: api/product/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductEntity([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isSuccess = await _productService.Delete(id);

            if (isSuccess)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}