using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Products.Web.Infrastructure.Entities;

namespace Products.Web.Infrastructure.Repositories
{
    public class Repository<T>: IRepository<T>
        where T : BaseEntity
    {
        #region Dependencies

        private readonly EfDbContext _context;

        #endregion Dependencies

        #region Fields

        private DbSet<T> _entities;

        #endregion Fields

        #region Ctors

        public Repository(EfDbContext context) => _context = context;

        #endregion Ctors

        #region Properties

        protected DbSet<T> Entities => _entities ?? (_entities = _context.Set<T>());

        #endregion Properties

        #region IRepository

        public virtual async Task<long> CreateAsync(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            var result = await Entities.AddAsync(entity);
            await SaveAsync();

            return result.Entity.Id;
        }

        public virtual async Task<T> GetItemByIdAsync(long id) => await Entities.FindAsync(id);

        public virtual IQueryable<T> GetItemList() => Entities.AsNoTracking().AsQueryable();

        public virtual void Update(T item)
        {
            Entities.Attach(item);
            var entry = _context.Entry(item);
            entry.State = EntityState.Modified;
        }

        public virtual async Task Remove(T item)
        {
            Entities.Remove(item);
            await SaveAsync();
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        #endregion IRepository
    }
}
