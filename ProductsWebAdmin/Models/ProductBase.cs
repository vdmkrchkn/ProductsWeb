using System.ComponentModel.DataAnnotations;

namespace ProductsWebAdmin.Models
{
    public class ProductBase
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public double Price { get; set; }
    }
}
