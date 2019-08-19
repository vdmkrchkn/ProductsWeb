using ProductsWebAdmin.Models;
using System.Threading.Tasks;

namespace ProductsWebAdmin.Services
{
    public interface IAuthService
    {
        Task<string> GetToken(User user);
    }
}
