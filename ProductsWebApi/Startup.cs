using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProductsWebApi.Models;
using ProductsWebApi.Models.Extensions;
using ProductsWebApi.Services;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ProductsWebApi
{
    public class Startup
    {
        private readonly string _corsPolicyName;
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _corsPolicyName = "allowAnyOrigin";
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            var appSettingsSection = _configuration.GetSection("ApplicationSettings");
            services.Configure<ApplicationSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<ApplicationSettings>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = appSettings.AuthToken.Issuer,

                        ValidateAudience = false,
                        ValidateLifetime = true,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(appSettings.AuthToken.Key)),
                        ValidateIssuerSigningKey = true,
                    };
                });

            var connection = _configuration.GetConnectionString("MsSqlLocalConnection");
            services.AddDbContext<EFDbContext>(options => options.UseSqlServer(connection));

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<IAuthService, JwtAuthService>();
            services.AddTransient<IUserService, UserService>();

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

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductsWeb API");
                    c.RoutePrefix = string.Empty;                    
                });
            }

            app.UseCors(_corsPolicyName);
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
