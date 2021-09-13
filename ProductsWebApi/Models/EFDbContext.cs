using Microsoft.EntityFrameworkCore;
using ProductsWebApi.Models.Entities;

namespace ProductsWebApi.Models
{
    public class EfDbContext : DbContext
    {
        #region Ctor

        public EfDbContext(DbContextOptions<EfDbContext> options) : base(options){ }

        #endregion Ctor

        #region DbContext

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class => base.Set<TEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductEntity>();
            modelBuilder.Entity<UserEntity>();
            modelBuilder.Entity<OrderEntity>();

            base.OnModelCreating(modelBuilder);
        }

        #endregion DbContext
    }
}
