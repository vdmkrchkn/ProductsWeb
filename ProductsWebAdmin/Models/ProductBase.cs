using System.ComponentModel.DataAnnotations;

namespace ProductsWebAdmin.Models
{
    public class ProductBase
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^(?:[1-9]\d*|0)?(?:\.\d{1,2})?$", ErrorMessage = "Invalid float format")]
        public string Price { get; set; }
    }
}
