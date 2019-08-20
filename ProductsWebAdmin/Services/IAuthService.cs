using ProductsWebAdmin.Models;
using System.Threading.Tasks;

namespace ProductsWebAdmin.Services
{
    public interface IAuthService
    {
        /// <summary>
        /// Make an async request to API and receive user token
        /// </summary>
        /// <param name="user">user's login credentials</param>
        /// <returns>JWT token as string. Or null if the request is invalid</returns>
        Task<string> GetToken(User user);
    }
}
