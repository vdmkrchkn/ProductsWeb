using System.ComponentModel.DataAnnotations;

namespace ProductsWebAdmin.Models
{
    public class User
    {
        [Required(ErrorMessage ="Must be entered")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Must be entered")]
        public string Password { get; set; }

        public override string ToString() => $"{Username}: {Password}";
    }
}
