using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private readonly EFDbContext _context;

        public ProductController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public IEnumerable<ProductEntity> GetProductEntity()
        {
            //var data = new List<ProductEntity>();
            //data.Add(new ProductEntity() { Id = 0, Name = "name", Price = 99.9 });
            return _context.Set<ProductEntity>();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductEntity([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productEntity = await _context.ProductEntity.SingleOrDefaultAsync(m => m.Id == id);

            if (productEntity == null)
            {
                return NotFound();
            }

            return Ok(productEntity);
        }

        // PUT: api/Product/5
        [HttpPut("{id}")]
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

            _context.Entry(productEntity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

        // POST: api/Product
        [HttpPost]
        public async Task<IActionResult> PostProductEntity([FromBody] ProductEntity productEntity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ProductEntity.Add(productEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductEntity", new { id = productEntity.Id }, productEntity);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductEntity([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productEntity = await _context.ProductEntity.SingleOrDefaultAsync(m => m.Id == id);
            if (productEntity == null)
            {
                return NotFound();
            }

            _context.ProductEntity.Remove(productEntity);
            await _context.SaveChangesAsync();

            return Ok(productEntity);
        }

        private bool ProductEntityExists(long id)
        {
            return _context.ProductEntity.Any(e => e.Id == id);
        }
    }
}