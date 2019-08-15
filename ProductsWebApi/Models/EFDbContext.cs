using Microsoft.EntityFrameworkCore;
using ProductsWebApi.Models.Entities;

namespace ProductsWebApi.Models
{
    public class EFDbContext : DbContext
    {
        #region Ctor

        public EFDbContext(DbContextOptions<EFDbContext> options) : base(options){ }

        #endregion Ctor

        #region DbContext

        public new DbSet<TEntity> Set<TEntity>() where TEntity : class => base.Set<TEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductEntity>();

            base.OnModelCreating(modelBuilder);
        }

        #endregion DbContext
    }
}
