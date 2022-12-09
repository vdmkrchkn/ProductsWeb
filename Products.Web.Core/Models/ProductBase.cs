using System.ComponentModel.DataAnnotations;

namespace Products.Web.Core.Models
{
    public class ProductBase : BaseJson
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Price { get; set; }
    }
}
