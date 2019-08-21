using ProductsWebAdmin.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ProductsWebAdmin.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductBase>> GetProducts(string name, double? priceMin, double? priceMax);

        Task<Product> GetProduct(long id);

        Task<HttpStatusCode> Edit(Product product, string token);

        Task<HttpStatusCode> Add(Product product, string token);
    }
}
