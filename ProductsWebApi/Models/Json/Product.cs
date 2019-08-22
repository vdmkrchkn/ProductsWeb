using Microsoft.AspNetCore.Http;

namespace ProductsWebApi.Models.Json
{
    public class Product : ProductBase
    {
        public string Description { get; set; }

        public string PictureName { get; set; }

        public IFormFile Image { get; set; }
    }
}
