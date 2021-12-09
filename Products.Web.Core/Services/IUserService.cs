using System.Threading.Tasks;
using User = Products.Web.Infrastructure.Entities.User;

namespace Products.Web.Core.Services
{
    public interface IUserService
    {
        User FindUserByName(string name);

        Task<long> Add(Models.User user, string role);
    }
}
