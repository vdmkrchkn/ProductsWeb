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
        public async Task<IActionResult> Edit([FromRoute] long id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!HttpContext.Request.Cookies.TryGetValue("token", out string token))
            {
                ModelState.AddModelError("", "invalid auth token");
                return RedirectToAction("Index", "Auth");
            }

            product.Id = id;

            HttpStatusCode statusCode = await _productService.Edit(product, token);

            switch(statusCode)
            {
                case HttpStatusCode.Unauthorized:
                {
                    ModelState.AddModelError("", "Unauthorized");
                    return RedirectToAction("Index", "Auth");
                }
                case HttpStatusCode.NoContent:
                    return RedirectToAction("Index");
                default:
                    return View(product);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit");
            }

            if (!HttpContext.Request.Cookies.TryGetValue("token", out string token))
            {
                ModelState.AddModelError("editProduct", "invalid auth token");
                return RedirectToAction("Index", "Auth");
            }

            HttpStatusCode statusCode = await _productService.Add(product, token);

            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    {
                        ModelState.AddModelError("addProduct", "Unauthorized");
                        return RedirectToAction("Index", "Auth");
                    }
                case HttpStatusCode.Created:
                    return RedirectToAction("Index");
                default:
                    return RedirectToAction("Edit");
            }
        }

        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            if (!HttpContext.Request.Cookies.TryGetValue("token", out string token))
            {
                ModelState.AddModelError("editProduct", "invalid auth token");
                return RedirectToAction("Index", "Auth");
            }

            HttpStatusCode statusCode = await _productService.Delete(id, token);

            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    {
                        ModelState.AddModelError("deleteProduct", "Unauthorized");
                        return RedirectToAction("Index", "Auth");
                    }
                default:
                    return RedirectToAction("Index");
            }
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
