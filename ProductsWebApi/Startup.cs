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
using System.Linq;
using System.Text;

namespace ProductsWebApi
{
    public class Startup
    {
        private readonly string _corsPolicyName;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _corsPolicyName = "allowAnyOrigin";
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            var appSettingsSection = Configuration.GetSection("ApplicationSettings");
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

            var connection = Configuration.GetConnectionString("MsSqlConnection");
            services.AddDbContext<EFDbContext>(options => options.UseSqlServer(connection));
            
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IAuthService, JwtAuthService>();
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(_corsPolicyName);
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
