using System.ComponentModel.DataAnnotations;

namespace Products.Web.Infrastructure.Entities
{
    public class PostOffice
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public bool IsWorking { get; set; }
    }
}
