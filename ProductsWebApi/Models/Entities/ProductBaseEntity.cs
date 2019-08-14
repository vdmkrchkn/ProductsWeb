using System.ComponentModel.DataAnnotations;

namespace ProductsWebApi.Models.Entities
{
    public class ProductBaseEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
