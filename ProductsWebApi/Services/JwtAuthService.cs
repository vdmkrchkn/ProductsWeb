using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;
using ProductsWebApi.Models;
using ProductsWebApi.Models.Extensions;
using ProductsWebApi.Models.Json;

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

        private ClaimsIdentity GetIdentity(User verifiedUser)
        {
            var user = _userService.FindUserByName(verifiedUser.Name);

            if (user != null) 
            {
                if (!user.IsPasswordValid(verifiedUser.Password)) return null;

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

            return null;
        }
    }
}
