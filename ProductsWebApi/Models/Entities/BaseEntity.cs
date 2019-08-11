using System.ComponentModel.DataAnnotations;

namespace ProductsWebApi.Models.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
