using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsWebApi.Models;
using ProductsWebApi.Models.Entities;
using ProductsWebApi.Models.Views;

namespace ProductsWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/product")]
    public class ProductController : Controller
    {
        private readonly IRepository<ProductEntity> _productRepository;

        public ProductController(IRepository<ProductEntity> productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/product
        [HttpGet]
        public IEnumerable<ProductBaseEntity> GetProductEntity(string name, double? priceMin, double? priceMax)
        {
            var products = _productRepository.GetItemList();

            if (name != null)
            {
                products = products.Where(product => product.Name.StartsWith(name));
            }

            if (priceMin.HasValue)
            {
                products = products.Where(product => product.Price >= priceMin.Value);
            }

            if (priceMax.HasValue)
            {
                products = products.Where(product => product.Price <= priceMax.Value);
            }

            return products;
        }

        // GET: api/product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductEntity([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productEntity = await _productRepository.GetItemById(id);

            if (productEntity == null)
            {
                return NotFound();
            }


            byte[] image = null;
            try
            {
                image = System.IO.File.ReadAllBytes(productEntity.PictureName);
            }
            catch (System.Exception)
            {

            }
            
            var product = new Product
            {
                Name = productEntity.Name,
                Price = productEntity.Price,
                Description = productEntity.Description,
                Image = image
            }; 

            return Ok(product);
        }

        // PUT: api/product/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutProductEntity([FromRoute] long id, [FromBody] ProductEntity productEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productEntity.Id)
            {
                return BadRequest();
            }

            _productRepository.Update(productEntity);

            try
            {
                await _productRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/product
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PostProductEntity([FromBody] ProductEntity productEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productRepository.Create(productEntity);

            return CreatedAtAction("GetProductEntity", new { id = productEntity.Id }, productEntity);
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

            var productEntity = await _productRepository.GetItemById(id);
            if (productEntity == null)
            {
                return NotFound();
            }

            await _productRepository.Remove(productEntity);

            return Ok(productEntity);
        }

        private bool ProductEntityExists(long id)
        {
            return _productRepository.GetItemList().Any(e => e.Id == id);
        }
    }
}