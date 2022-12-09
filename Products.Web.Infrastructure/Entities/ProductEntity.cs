using System.ComponentModel.DataAnnotations.Schema;

namespace Products.Web.Infrastructure.Entities
{
    [Table("Products")]
    public class ProductEntity : ProductBaseEntity
    {
        public string Description { get; set; }

        public string PictureName { get; set; }
    }
}
