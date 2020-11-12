using System.Threading.Tasks;
using ProductsWebApi.Models.Entities;
using ProductsWebApi.Models.Json;

namespace ProductsWebApi.Services
{
    public interface IUserService
    {
        UserEntity FindUserByName(string name);

        Task<long> Add(User user, string role);
    }
}
