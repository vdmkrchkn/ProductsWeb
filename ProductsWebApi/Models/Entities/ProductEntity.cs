using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsWebApi.Models.Entities
{
    [Table("Products")]
    public class ProductEntity : ProductBaseEntity
    {
        public string Description { get; set; }

        public string PictureName { get; set; }
    }
}
