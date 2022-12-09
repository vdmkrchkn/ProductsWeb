using System.Linq;
using System.Threading.Tasks;
using Products.Web.Infrastructure.Entities;

namespace Products.Web.Infrastructure.Repositories
{
    public interface IRepository<T>
        where T : BaseEntity
    {
        // получить список сущностей
        // todo: добавить фильтры, преобразовать в async
        IQueryable<T> GetItemList();
        // получить сущность по id
        Task<T> GetItemByIdAsync(long id);
        // создать сущность и вернуть id
        Task<long> CreateAsync(T item);
        // обновить сущность
        void Update(T item);
        // удалить сущность
        Task Remove(T item);
        // сохранить все изменения контекста
        Task SaveAsync();
    }
}
