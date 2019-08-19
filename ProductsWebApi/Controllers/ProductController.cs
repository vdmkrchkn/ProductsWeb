using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsWebApi.Models;
using ProductsWebApi.Models.Entities;
using ProductsWebApi.Models.Json;

namespace ProductsWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/product")]
    public class ProductController : Controller
    {
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IRepository<ProductEntity> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;

        }

        // GET: api/product
        [HttpGet]
        public IEnumerable<ProductBase> GetProductEntity(string name, double? priceMin, double? priceMax)
        {
            var productEntities = _productRepository.GetItemList();

            if (name != null)
            {
                productEntities = productEntities.Where(product => product.Name.StartsWith(name));
            }

            if (priceMin.HasValue)
            {
                productEntities = productEntities.Where(product => product.Price >= priceMin.Value);
            }

            if (priceMax.HasValue)
            {
                productEntities = productEntities.Where(product => product.Price <= priceMax.Value);
            }

            List<ProductBase> products = new List<ProductBase>();
            foreach (var product in productEntities)
            {
                products.Add(new ProductBase { Name = product.Name, Price = product.Price });
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

            try
            {
                var product = _mapper.Map<Product>(productEntity);

                if (productEntity.PictureName != null)
                {
                    var filePath = Path.Combine("Images", productEntity.PictureName);
                    product.Image = System.IO.File.ReadAllBytes(filePath);
                }

                return Ok(product);
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e);

                return BadRequest();
            }
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

            try
            {
                var productEntity = _mapper.Map<ProductEntity>(product);
                productEntity.Id = id;
                productEntity.PictureName = SaveImage(product);

                _productRepository.Update(productEntity);

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
        public async Task<IActionResult> PostProductEntity([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productEntity = _mapper.Map<ProductEntity>(product);
            productEntity.PictureName = SaveImage(product);

            await _productRepository.Create(productEntity);

            return CreatedAtAction("PostProductEntity", new { id = productEntity.Id }, product);
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

            try
            {
                System.IO.File.Delete(productEntity.PictureName);
            }
            catch(System.Exception e)
            {
                Debug.WriteLine(e);
            }
            
            await _productRepository.Remove(productEntity);

            return Ok(productEntity);
        }

        private bool ProductEntityExists(long id)
        {
            return _productRepository.GetItemList().Any(e => e.Id == id);
        }

        private string SaveImage(Product product)
        {
            string fileName = null;

            if (product.Image != null && product.Image.Length > 0)
            {
                fileName = Path.GetRandomFileName(); //$"{id}_{product.Name}.webp";
                
                // преобразование набора байтов в изображение
                System.IO.File.WriteAllBytes(fileName, product.Image);
            }

            return fileName;
        }
    }
}