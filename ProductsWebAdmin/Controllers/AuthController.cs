using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using ProductsWebAdmin.Models;
using ProductsWebAdmin.Services;
using System.Threading.Tasks;

namespace ProductsWebAdmin.Controllers
{
    public class AuthController : Controller
    {        
        private readonly IAuthService _authService;
        private const string COOKIE_TOKEN_KEY = "token";

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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

            string token = await _authService.GetToken(user);

            if (!string.IsNullOrEmpty(token))
            {
                Response.Cookies.Append(COOKIE_TOKEN_KEY, token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("InvalidCredentials", "Invalid username or password");
            }

            return View(user);
        }

        public IActionResult Logout()
        {
            //await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
            Response.Cookies.Delete(COOKIE_TOKEN_KEY);

            return RedirectToAction("Index");
        }
    }
}