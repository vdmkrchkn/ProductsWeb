using Microsoft.Extensions.Options;
using ProductsWebAdmin.Models.Extensions;
using System;
using System.Net.Http;

namespace ProductsWebAdmin.Services
{
    public class HttpClientProvider
    {
        public HttpClientProvider(IOptions<ApplicationSettings> appSettings)
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(appSettings.Value.ApiUrl),
            };
        }

        public HttpClient Client { get; private set; }        
    }
}
