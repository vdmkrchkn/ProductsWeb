using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProductsWebApi.Models;
using ProductsWebApi.Models.Extensions;
using ProductsWebApi.Models.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace ProductsWebApi.Services
{
    public class JwtAuthService : IAuthService
    {
        private readonly IList<Models.Extensions.User> _users;
        private readonly AuthOptions _token;

        public JwtAuthService(IOptions<ApplicationSettings> appSettings)
        {
            _users = appSettings.Value.Users;
            _token = appSettings.Value.AuthToken;
        }
        
        public AuthToken GetToken(Models.Json.User user)
        {
            var identity = GetIdentity(user);

            if (identity == null)
            {
                return null;
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

            var authToken = new AuthToken
            {
                Username = identity.Name,
                Token = encodedJwt,
            };

            return authToken;
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
