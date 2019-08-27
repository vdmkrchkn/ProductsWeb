using System.ComponentModel.DataAnnotations;

namespace ProductsWebApi.Models.Json
{
    public class ProductBase
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
