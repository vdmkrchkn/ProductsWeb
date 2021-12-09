using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products.Web.Core.Services
{
    using Models;

    public interface IProductService
    {
        IEnumerable<ProductBase> GetProducts(ProductSearchFilter filter);

        Task<Product> GetProductById(long id);

        Task<bool> Edit(Product product);

        Task<bool> Add(Product product);

        Task<bool> Delete(long id);
    }
}
