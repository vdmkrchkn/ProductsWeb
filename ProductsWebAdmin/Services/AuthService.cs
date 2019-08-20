using Newtonsoft.Json;
using ProductsWebAdmin.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProductsWebAdmin.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _client; // TODO: use as singleton

        public AuthService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:31549/"), // TODO: use config
            };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        public async Task<string> GetToken(User user)
        {
            var userContent = JsonConvert.SerializeObject(user);
            var byteContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(userContent));
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _client.PostAsync(
                "api/auth", byteContent);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();

                var authToken = JsonConvert.DeserializeObject<AuthToken>(
                    data, new JsonSerializerSettings { Formatting = Formatting.Indented });

                return authToken.Token;
            }

            return null;
        }
    }
}
