using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductsWebApi.Models;
using System;

namespace ProductsWebApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Migrate database.
        /// </summary>
        /// <param name="app"></param>
        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<EfDbContext>().Database;
                db.SetCommandTimeout(TimeSpan.FromMinutes(5));
                db.Migrate();

                using (var connection = db.GetDbConnection())
                {
                    connection.Open();
                }
            }
        }
    }
}
