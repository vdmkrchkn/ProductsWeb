using System.ComponentModel.DataAnnotations;

namespace Products.Web.Infrastructure.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public long Id { get; set; }
    }
}
