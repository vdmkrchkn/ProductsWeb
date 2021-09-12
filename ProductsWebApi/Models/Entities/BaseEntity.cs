using System.ComponentModel.DataAnnotations;

namespace ProductsWebApi.Models.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public long Id { get; set; }
    }
}
