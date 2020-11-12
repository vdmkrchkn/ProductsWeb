using System;
using System.Linq;
using System.Threading.Tasks;
using ProductsWebApi.Models;
using ProductsWebApi.Models.Entities;
using ProductsWebApi.Models.Json;

namespace ProductsWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserEntity> _userRepository;

        public UserService(IRepository<UserEntity> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<long> Add(User user, string role)
        {
            if (FindUserByName(user.Name) != null)
            {
                throw new Exception($"There is a registered user with name \'{user.Name}\'");
            }

            return await _userRepository.Create(new UserEntity (
                user.Name,
                user.Password,
                role
            ));
        }

        public UserEntity FindUserByName(string name)
        {
            var users = _userRepository.GetItemList();
            return users.FirstOrDefault(user => user.Name == name);
        }
    }
}
