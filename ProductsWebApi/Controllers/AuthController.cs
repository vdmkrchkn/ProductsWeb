using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductsWebApi.Services;
using System.Threading.Tasks;

namespace ProductsWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task Verify([FromBody] Models.Json.User user)
        {
            if (user == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid request body format");
                return;
            }

            var token = _authService.GetToken(user);

            if (token == null)
            {
                Response.StatusCode = 401;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }

            // answer serialization
            Response.ContentType = "application/json";
            await Response.WriteAsync(
                JsonConvert.SerializeObject(
                    token, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }
    }
}