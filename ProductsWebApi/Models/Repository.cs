using ProductsWebApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsWebApi.Models
{
    public class Repository<T>: IRepository<T>
        where T : BaseEntity
    {
        #region Dependencies  

        private readonly EFDbContext _context;

        #endregion Dependencies  

        #region Fields

        private DbSet<T> entities;

        #endregion Fields

        #region Ctors

        public Repository(EFDbContext context)
        {
            _context = context;
        }

        #endregion Ctors

        #region Properties

        private DbSet<T> Entities
        {
            get
            {
                if (entities == null)
                {
                    entities = _context.Set<T>();
                }

                return entities;
            }
        }

        #endregion Properties

        #region IRepository

        public async Task Create(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            await Entities.AddAsync(entity);
            await Save();
        }        

        public async Task<T> GetItemById(long id)
        {
            return await Entities.FindAsync(id);
        }        

        public IEnumerable<T> GetItemList()
        {
            return Entities.AsEnumerable();
        }

        public void Update(T item)
        {
            Entities.Attach(item);
            var entry = _context.Entry(item);
            entry.State = EntityState.Modified;
        }

        public async Task Remove(T item)
        {
            Entities.Remove(item);
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }        

        #endregion IRepository
    }
}
