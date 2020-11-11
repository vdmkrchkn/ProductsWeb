using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductsWebApi.Models.Json;
using ProductsWebApi.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ProductsWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(
            ILogger<AuthController> logger,
            IAuthService authService,
            IUserService userService)
        {
            _logger = logger;
            _authService = authService;
            _userService = userService;
        }

        /// <summary>
        /// Verify user credentials
        /// </summary>
        /// <param name="user">User credentials</param>
        /// <returns>JWT token</returns>
        /// <response code="200">Returns user JWT token data</response>
        /// <response code="400">If the user is null</response>
        /// <response code="401">If the user is invalid</response>
        [HttpPost]
        [ProducesResponseType(typeof(AuthToken), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task Verify([FromBody] User user)
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

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="user">User credentials</param>
        /// <returns></returns>
        /// <response code="200">User has registered successfully</response>
        /// <response code="400">If the user is invalid</response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.Add(user, "admin");

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "");
                return BadRequest();
            }
        }
    }
}