using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ProductsWebApi.Models.Entities
{
    [Table("Users")]
    public class UserEntity : BaseEntity
    {
        public UserEntity() { }
        public UserEntity(string name, string password, string role)
        {
            Name = name;
            
            byte[] salt = GetPseudorandomByteArray();

            Password = GetHashString(password, salt);
            Salt = Convert.ToBase64String(salt);
            Role = role;
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public string Role { get; set; }

        public bool IsPasswordValid(string password) => 
            Password == GetHashString(password, Convert.FromBase64String(Salt));
        
        private static byte[] GetPseudorandomByteArray()
        {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }
        
        private static string GetHashString(string str, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: str,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32
            ));
        }
    }
}
