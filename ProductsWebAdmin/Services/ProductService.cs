using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task<HttpStatusCode> Add(Product product, string token)
        {
            var productContent = JsonConvert.SerializeObject(product);
            var byteContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(productContent));
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.PostAsync($"api/product", byteContent);

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> Edit(Product product, string token)
        {            
            HttpContent productContent;

            if (product.Image != null)
            {
                byte[] imageData;
                using (var br = new BinaryReader(product.Image.OpenReadStream()))
                {
                    imageData = br.ReadBytes((int)product.Image.OpenReadStream().Length);
                }
              
                var imageBytes = new ByteArrayContent(imageData);

                productContent = new MultipartFormDataContent
                {
                    { imageBytes, "Image", product.Image.FileName },
                    { new StringContent(product.Id.ToString()), "Id" },
                    { new StringContent(product.Name), "Name" },
                    { new StringContent(product.Price.ToString()), "Price" },
                    { new StringContent(product.Description), "Description" }
                };
            }
            else
            {
                var productJson = JsonConvert.SerializeObject(product);
                productContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(productJson));
                productContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.PutAsync($"api/product/{product.Id}", productContent);

            return response.StatusCode;
        }
    }
}
