using ProductsWebApi.Models.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsWebApi.Services
{
    public interface IProductService
    {
        IEnumerable<ProductBase> GetProducts(string name, double? priceMin, double? priceMax);

        Task<Product> GetProduct(long id);

        Task<bool> Edit(Product product);

        Task<bool> Add(Product product);

        Task<bool> Delete(long id);
    }
}
