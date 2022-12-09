using Products.Web.Core.Models;

namespace Products.Web.Core.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Verify user's credentials by providing token
        /// </summary>
        /// <param name="user">user's login credentials</param>
        /// <returns>JWT token object. Or null unless user is valid</returns>
        AuthToken GetToken(User user);
    }
}
