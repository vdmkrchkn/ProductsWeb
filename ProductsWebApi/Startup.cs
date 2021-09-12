using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsWebApi.Extensions;
using ProductsWebApi.Models.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ProductsWebApi
{
    public class Startup
    {
        private const string _corsPolicyName = "allowAnyOrigin";
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            var appSettingsSection = _configuration.GetSection(nameof(ApplicationSettings));
            services.Configure<ApplicationSettings>(appSettingsSection);

            services.ConfigureAuth(appSettingsSection.Get<ApplicationSettings>().AuthToken);

            services.ConfigureDatabase(_configuration);

            services.AddAutoMapper(typeof(Startup));

            services.AddCors(options => options.AddPolicy(
                _corsPolicyName,
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            ));

            services.AddMvc(options =>
            {
                var jsonInputFormatter = options.InputFormatters.OfType<JsonInputFormatter>().First();
                jsonInputFormatter.SupportedMediaTypes.Add("multipart/form-data");
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ProductsWeb API", Version = "v1" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.MigrateDatabase();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductsWeb API");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors(_corsPolicyName);
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
