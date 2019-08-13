using System.ComponentModel.DataAnnotations;

namespace ProductsWebApi.Models.Entities
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }
    }
}
