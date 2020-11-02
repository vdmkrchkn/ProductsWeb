using ProductsWebApi.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsWebApi.Models
{
    public interface IRepository<T>
        where T : BaseEntity
    {        
        // получить список сущностей
        IEnumerable<T> GetItemList();
        // получить сущность по id
        Task<T> GetItemById(long id);
        // создать сущность и вернуть id
        Task<long> Create(T item);
        // обновить сущность
        void Update(T item);
        // удалить сущность
        Task Remove(T item);
        // сохранить все изменения контекста
        Task Save();
    }
}
