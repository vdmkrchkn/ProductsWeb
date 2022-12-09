using System;
using System.Linq;
using System.Threading.Tasks;
using Products.Web.Infrastructure.Repositories;
using User = Products.Web.Infrastructure.Entities.User;

namespace Products.Web.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<long> Add(Models.User user, string role)
        {
            if (FindUserByName(user.Name) != null)
            {
                throw new Exception($"There is a registered user with name \'{user.Name}\'");
            }

            return await _userRepository.CreateAsync(new User (
                user.Name,
                user.Password,
                role
            ));
        }

        public User FindUserByName(string name)
        {
            var users = _userRepository.GetItemList();
            return users.FirstOrDefault(user => user.Name == name);
        }
    }
}
