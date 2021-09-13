using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProductsWebApi.Models;
using ProductsWebApi.Services;
using System;
using System.Text;

namespace ProductsWebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure database
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="config"><see cref="IConfiguration"/></param>
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<EfDbContext>(options =>
            {
                var connection = config.GetConnectionString("DBConnection");
                options.OnNpgsqlConfiguring(connection);
            },
                ServiceLifetime.Scoped,
                ServiceLifetime.Singleton);

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            #region Services

            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IAuthService, JwtAuthService>();
            services.AddTransient<IUserService, UserService>();

            #endregion

            return services;
        }

        private static DbContextOptionsBuilder OnMsSqlConfiguring(this DbContextOptionsBuilder optionsBuilder, string connection)
            => optionsBuilder.UseSqlServer(connection, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });

        private static DbContextOptionsBuilder OnNpgsqlConfiguring(this DbContextOptionsBuilder optionsBuilder, string connection)
            => optionsBuilder.UseNpgsql(connection)
                .EnableSensitiveDataLogging();

        public static IServiceCollection ConfigureAuth(this IServiceCollection services, AuthOptions applicationSettings)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = applicationSettings.Issuer,

                        ValidateAudience = false,
                        ValidateLifetime = true,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(applicationSettings.Key)),
                        ValidateIssuerSigningKey = true,
                    };
                });

            return services;
        }
    }
}
