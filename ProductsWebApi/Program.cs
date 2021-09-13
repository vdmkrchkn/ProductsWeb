using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ProductsWebApi
{
    public class Program
    {
        public static void Main(string[] args) => BuildWebHost(args).Run();

        private static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(
                    (builderContext, config) =>
                    {
                        config.AddEnvironmentVariables();
                    }
                )
                .UseStartup<Startup>()
                .Build();
    }
}
