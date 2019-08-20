using Microsoft.AspNetCore.Mvc;
using ProductsWebAdmin.Filters;
using ProductsWebAdmin.Models;
using ProductsWebAdmin.Services;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace ProductsWebAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        [AuthFilter]
        public async Task<IActionResult> Index([FromQuery] string name, [FromQuery] double priceMin, [FromQuery] double priceMax)
        {
            var products = await _productService.GetProducts(name, priceMin, priceMax);

            return View(products);
        }

        [AuthFilter]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id.HasValue)
            {
                ViewData["Title"] = "Edit";

                var product = await _productService.GetProduct(id.Value);

                return View(product);
            }

            ViewData["Title"] = "Add";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm]Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!HttpContext.Request.Cookies.TryGetValue("token", out string token))
            {
                ModelState.AddModelError("editProduct", "invalid auth token");
                return RedirectToAction("Index", "Auth");
            }

            HttpStatusCode statusCode = await _productService.Edit(product, token);

            if (statusCode == HttpStatusCode.Unauthorized)
            {
                ModelState.AddModelError("editProduct", "Unauthorized");
                return RedirectToAction("Index", "Auth");
            }

            return RedirectToAction("Index");
        }

        [AuthFilter]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [AuthFilter]
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
