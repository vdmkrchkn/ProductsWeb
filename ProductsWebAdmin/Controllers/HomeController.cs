using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsWebAdmin.Models;
using System.Diagnostics;

namespace ProductsWebAdmin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //Debug.WriteLine($"User.Identity = {User.Identity}");
            if (HttpContext.Request.Cookies.ContainsKey("token"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
