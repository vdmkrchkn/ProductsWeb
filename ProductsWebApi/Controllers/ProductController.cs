using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Products.Web.Core.Models;
using Products.Web.Core.Services;

namespace ProductsWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/product")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService) => _productService = productService;

        // GET: api/product
        [HttpGet]
        public IEnumerable<ProductBase> GetProducts(string name, decimal? priceMin, decimal? priceMax) =>
            _productService.GetProducts(new ProductSearchFilter(name, priceMin, priceMax));

        // GET: api/product/5
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetProductById([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productService.GetProductById(id);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/product/5
        [HttpPut("{id:long}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeProduct([FromRoute] long id, [FromBody] Product product)
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

            return BadRequest();
        }

        // PUT: api/product/image/5
        [HttpPut("image/{id:long}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ChangeProduct(
            [FromRoute] long id,
            IFormFile image,
            Product product)
        {
            product.Image = image;

            return await ChangeProduct(id, product);
        }

        // POST: api/product
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _productService.Add(product))
            {
                return CreatedAtAction("AddProduct", new { id = product.Id }, product);
            }
            else
            {
                return StatusCode(500, "internal error");
            }
        }

        // POST: api/product/image
        [HttpPost("image")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddProductWithImage(IFormFile image, Product product)
        {
            product.Image = image;

            return await AddProduct(product);
        }

        // DELETE: api/product/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isSuccess = await _productService.Delete(id);

            if (isSuccess)
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
