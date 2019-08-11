using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsWebApi.Models;
using ProductsWebApi.Models.Entities;

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
        public IEnumerable<ProductEntity> GetProductEntity()
        {
            return _productRepository.GetItemList();
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

            return Ok(productEntity);
        }

        // PUT: api/product/5
        [HttpPut("{id}")] // add token
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
        [HttpPost]  // add token
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
        [HttpDelete("{id}")] // add token
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

            _productRepository.Remove(productEntity);

            return Ok(productEntity);
        }

        private bool ProductEntityExists(long id)
        {
            return _productRepository.GetItemList().Any(e => e.Id == id);
        }
    }
}