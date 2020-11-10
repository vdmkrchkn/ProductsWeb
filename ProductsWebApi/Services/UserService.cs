using System;
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

        public async Task<long> Add(UserEntity user)
        {
            if (FindUserByName(user.Name) != null)
            {
                throw new Exception($"There is a registered user with name \'{user.Name}\'");
            }

            return await _userRepository.Create(user);
        }

        public UserEntity FindUserByName(string name)
        {
            var users = _userRepository.GetItemList();
            return users.FirstOrDefault(user => user.Name == name);
        }
    }
}
