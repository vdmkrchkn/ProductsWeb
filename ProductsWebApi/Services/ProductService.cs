using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Products.Web.Infrastructure;
using Products.Web.Infrastructure.Entities;
using Products.Web.Infrastructure.Repositories;

namespace ProductsWebApi.Services
{
    using Products.Web.Core;
    using Products.Web.Core.Models;
    using Products.Web.Core.Services;

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

        public IEnumerable<ProductBase> GetProducts(ProductSearchFilter filter)
        {
            var productEntities = _productRepository.GetItemList();

            if (filter.Name != null)
            {
                productEntities = productEntities.Where(product => product.Name.StartsWith(filter.Name));
            }

            if (filter.PriceMin.HasValue)
            {
                productEntities = productEntities.Where(product => product.Price >= filter.PriceMin.Value);
            }

            if (filter.PriceMax.HasValue)
            {
                productEntities = productEntities.Where(product => product.Price <= filter.PriceMax.Value);
            }

            return productEntities.Select(product => new ProductBase
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price.ToString(CultureInfo.InvariantCulture)
            }).ToList();
        }

        public async Task<Product> GetProductById(long id)
        {
            var productEntity = await _productRepository.GetItemByIdAsync(id);
            await Task.Delay(1000);

            if (productEntity is null) return null;

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

                await _productRepository.CreateAsync(productEntity);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
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

                await _productRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductEntityExists(product.Id))
                {
                    return false;
                }

                throw;
            }

            return true;
        }

        public async Task<bool> Delete(long id)
        {
            var productEntity = await _productRepository.GetItemByIdAsync(id);

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

                using var stream = new FileStream(filePath, FileMode.Create);
                await product.Image.CopyToAsync(stream);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }

            return fileName;
        }
    }
}
