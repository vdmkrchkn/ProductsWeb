using System.ComponentModel.DataAnnotations;

namespace ProductsWebAdmin.Models
{
    public class User
    {
        [Required(ErrorMessage ="Must be entered")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Must be entered")]
        public string Password { get; set; }

        public override string ToString() => $"{Name}: {Password}";
    }
}
