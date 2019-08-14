using System.Collections.Generic;

namespace ProductsWebApi.Models.Extensions
{
    public class ApplicationSettings
    {
        public AuthOptions AuthToken { get; set; }

        // collection of authorized users
        public IList<User> Users { get; set; }
    }
}
