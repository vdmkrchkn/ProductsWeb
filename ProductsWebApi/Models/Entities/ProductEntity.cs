using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsWebApi.Models.Entities
{
    [Table("Products")]
    public class ProductEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
