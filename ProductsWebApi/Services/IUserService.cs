using System.Threading.Tasks;
using ProductsWebApi.Models.Entities;

namespace ProductsWebApi.Services
{
    public interface IUserService
    {
        UserEntity FindUserByName(string name);

        Task<bool> Add(UserEntity user);
    }
}
