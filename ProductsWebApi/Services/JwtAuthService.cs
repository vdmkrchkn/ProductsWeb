using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Products.Web.Core.Models;
using Products.Web.Core.Services;
using ProductsWebApi.Models;
using ProductsWebApi.Models.Extensions;

namespace ProductsWebApi.Services
{
    public class JwtAuthService : IAuthService
    {
        private readonly AuthOptions _token;
        private readonly IUserService _userService;

        public JwtAuthService(
            IOptions<ApplicationSettings> appSettings,
            IUserService userService)
        {
            _token = appSettings.Value.AuthToken;
            _userService = userService;
        }

        public AuthToken GetToken(User user)
        {
            var identity = GetIdentity(user);

            if (identity is null)
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

            return new AuthToken
            {
                Username = identity.Name,
                Token = encodedJwt,
            };
        }

        private ClaimsIdentity GetIdentity(User verifiedUser)
        {
            var user = _userService.FindUserByName(verifiedUser.Name);

            if (user is null || !user.IsPasswordValid(verifiedUser.Password)) return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
            };

            return new ClaimsIdentity(claims, "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
            );
        }
    }
}
