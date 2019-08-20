using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProductsWebAdmin.Models;

namespace ProductsWebAdmin.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client; // TODO: use as singleton

        public ProductService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:31549/"), // TODO: use config
            };
        }

        public async Task<IEnumerable<ProductBase>> GetProducts(string name, double? priceMin, double? priceMax)
        {
            HttpResponseMessage response = await _client.GetAsync("api/product");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();

                var products = JsonConvert.DeserializeObject<IEnumerable<ProductBase>>(
                    data, new JsonSerializerSettings { Formatting = Formatting.Indented });

                return products;
            }

            return null;
        }

        public async Task<Product> GetProduct(long id)
        {
            HttpResponseMessage response = await _client.GetAsync($"api/product/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();

                var product = JsonConvert.DeserializeObject<Product>(
                    data, new JsonSerializerSettings { Formatting = Formatting.Indented });

                return product;
            }

            return null;
        }

        public async Task<HttpStatusCode> Edit(Product product, string token)
        {
            var productContent = JsonConvert.SerializeObject(product);
            var byteContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(productContent));
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.PutAsync($"api/product/{product.Id}", byteContent);

            return response.StatusCode;
        }
    }
}
