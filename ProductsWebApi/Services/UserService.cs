using System.Linq;
using System.Threading.Tasks;
using ProductsWebApi.Models;
using ProductsWebApi.Models.Entities;

namespace ProductsWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserEntity> _userRepository;

        public UserService(IRepository<UserEntity> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Add(UserEntity user)
        {
            await _userRepository.Create(user);
            return true;
        }

        public UserEntity FindUserByName(string name)
        {
            var users = _userRepository.GetItemList();
            return users.FirstOrDefault(user => user.Name == name);
        }
    }
}
