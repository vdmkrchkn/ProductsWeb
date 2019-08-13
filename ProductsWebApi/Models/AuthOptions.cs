using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProductsWebApi.Models
{
    public struct AuthOptions
    {
        // token publisher
        public const string ISSUER = "ProductsWebApi";

        // token consumer
        public const string AUDIENCE = "http://localhost:31549/";

        private const string KEY = "my_super-puper_secret-key!123";

        // token time to life in minutes
        public const int LIFETIME = 5;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
