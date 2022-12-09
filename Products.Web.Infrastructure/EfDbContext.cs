using Microsoft.EntityFrameworkCore;
using Products.Web.Infrastructure.Entities;

namespace Products.Web.Infrastructure
{
    /// <summary>
    /// Стандартный контекст данных.
    /// </summary>
    public class EfDbContext : DbContext
    {
        #region Ctor

        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options){ }

        #endregion Ctor

        #region DbContext

        public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity => base.Set<TEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductEntity>();
            modelBuilder.Entity<User>().HasKey(entity => entity.Id);
            modelBuilder.Entity<User>().Property(entity => entity.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<OrderEntity>();
            modelBuilder.Entity<PostOffice>().HasKey(entity => entity.Name);

            base.OnModelCreating(modelBuilder);
        }

        #endregion DbContext
    }
}
