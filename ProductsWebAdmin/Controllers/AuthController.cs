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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm]User user)
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
                ModelState.AddModelError("", "Unauthorized");
            }

            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete(COOKIE_TOKEN_KEY);

            return RedirectToAction("Index");
        }
    }
}