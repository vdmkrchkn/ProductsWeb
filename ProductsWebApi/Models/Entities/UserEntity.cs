using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsWebApi.Models.Entities
{
    [Table("Users")]
    public class UserEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public string Role { get; set; }

        public bool IsPasswordValid(string password) => Password == password;
    }
}
