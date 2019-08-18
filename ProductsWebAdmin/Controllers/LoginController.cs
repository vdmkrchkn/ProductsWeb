using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductsWebAdmin.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProductsWebAdmin.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _client;

        public LoginController()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:31549/"),
            };

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]        
        public async Task<IActionResult> Index(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userContent = JsonConvert.SerializeObject(user);
            var byteContent = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(userContent));
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _client.PostAsync(
                "api/auth", byteContent);

            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync();

                // access_token в sessionStorage
                Debug.WriteLine(data);

                return Ok();
            }
            else
            {
                return Index();
            }
            
        }
    }
}