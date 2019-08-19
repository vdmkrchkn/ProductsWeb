using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using ProductsWebApi.Models;
using ProductsWebApi.Models.Extensions;
using ProductsWebApi.Models.Json;

namespace ProductsWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IList<Models.Extensions.User> _users;
        private readonly AuthOptions _token;

        public AuthController(IOptions<ApplicationSettings> appSettings)
        {            
            _users = appSettings.Value.Users;
            _token = appSettings.Value.AuthToken;
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

            var identity = GetIdentity(user);
            if (identity == null)
            {
                Response.StatusCode = 401;
                await Response.WriteAsync("Invalid username or password.");
                return;
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: _token.Issuer,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(_token.Lifetime)),
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_token.Key)), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new AuthToken
            {
                Username = identity.Name,
                Token = encodedJwt,
            };

            // answer serialization
            Response.ContentType = "application/json";
            await Response.WriteAsync(
                JsonConvert.SerializeObject(
                    response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private ClaimsIdentity GetIdentity(Models.Json.User verifiedUser)
        {
            var person = _users.FirstOrDefault(user => 
                user.Login == verifiedUser.Username && user.Password == verifiedUser.Password);
            
            if (person != null)
            {                
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
                };

                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}