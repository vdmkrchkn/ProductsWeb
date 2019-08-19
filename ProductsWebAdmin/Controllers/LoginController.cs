using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductsWebAdmin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
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
                var data = await response.Content.ReadAsStringAsync();

                Debug.WriteLine(data);

                HttpContext.Response.Cookies.Append("token", data);

                // создаем один claim
                //var claims = new List<Claim>
                //{
                //    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username)
                //};

                //// создаем объект ClaimsIdentity
                //var id = new ClaimsIdentity(claims, "ApplicationCookie",
                //    ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                //// установка аутентификационных куки
                //await HttpContext.SignInAsync(
                //    CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

                return RedirectToRoute("default");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
            }

            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}