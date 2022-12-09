using System.ComponentModel.DataAnnotations;

namespace Products.Web.Infrastructure.Entities
{
    public class ProductBaseEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
