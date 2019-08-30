using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ProductsWebApi.Models;
using ProductsWebApi.Models.Entities;
using ProductsWebApi.Models.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsWebApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IMapper _mapper;

        private readonly string _rootPath;

        public ProductService(IHostingEnvironment env, IRepository<ProductEntity> productRepository,
            IMapper mapper)
        {
            _rootPath = env.ContentRootPath;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public IEnumerable<ProductBase> GetProducts(string name, double? priceMin, double? priceMax)
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

            var products = new List<ProductBase>();
            foreach (var product in productEntities)
            {
                products.Add(new ProductBase
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price.ToString()
                });
            }

            return products;
        }

        public async Task<Product> GetProduct(long id)
        {
            ProductEntity productEntity = await _productRepository.GetItemById(id);            

            try
            {
                var product = _mapper.Map<Product>(productEntity);

                if (product.PictureName != null)
                {
                    product.PictureName =
                        Path.Combine(_rootPath, "Images", product.PictureName);
                }

                return product;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);

                return null;
            }
        }

        public async Task<bool> Add(Product product)
        {
            try
            {
                var productEntity = _mapper.Map<ProductEntity>(product);

                productEntity.PictureName = await SaveImage(product);

                await _productRepository.Create(productEntity);                
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }       

        public async Task<bool> Edit(Product product)
        {
            try
            {
                var productEntity = _mapper.Map<ProductEntity>(product);

                productEntity.PictureName = await SaveImage(product);

                _productRepository.Update(productEntity);

                await _productRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductEntityExists(product.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public async Task<bool> Delete(long id)
        {
            var productEntity = await _productRepository.GetItemById(id);

            if (productEntity == null)
            {
                return false;
            }

            if (productEntity.PictureName != null)
            {
                try
                {
                    string filePath = Path.Combine(_rootPath, "Images", productEntity.PictureName);
                    File.Delete(filePath);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }

            await _productRepository.Remove(productEntity);

            return true;

        }

        private bool ProductEntityExists(long id)
        {
            return _productRepository.GetItemList().Any(e => e.Id == id);
        }

        private async Task<string> SaveImage(Product product)
        {            
            if (product.Image == null || product.Image.Length == 0)
            {
                return null;
            }

            string fileName = product.Image.FileName;

            try
            {
                string filePath = Path.Combine(_rootPath, "Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.Image.CopyToAsync(stream);
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }

            return fileName;
        }
    }
}
