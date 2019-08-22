using Microsoft.AspNetCore.Http;

namespace ProductsWebAdmin.Models
{
    public class Product : ProductBase
    {
        public string Description { get; set; }

        public string PictureName { get; set; }

        public IFormFile Image { get; set; }
    }
}
