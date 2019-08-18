using System.ComponentModel.DataAnnotations;

namespace ProductsWebAdmin.Models
{
    public class User
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public override string ToString() => $"{Username}: {Password}";
    }
}
