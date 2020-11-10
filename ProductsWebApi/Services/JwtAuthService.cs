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
        private ILogger<JwtAuthService> _logger;

        public JwtAuthService(
            IOptions<ApplicationSettings> appSettings,
            ILogger<JwtAuthService> logger,
            IUserService userService)
        {
            _token = appSettings.Value.AuthToken;
            _logger = logger;
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

        public async Task<bool> AddUser(User user)
        {
            try
            {
                byte[] salt = GetPseudorandomByteArray();
                
                await _userService.Add(new Models.Entities.UserEntity {
                    Name = user.Name,
                    Password = GetHashString(user.Password, salt),
                    Salt = Convert.ToBase64String(salt),
                    Role = "admin"
                });

                return true;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "");
            }

            return false;
        }

        private ClaimsIdentity GetIdentity(User verifiedUser)
        {
            var user = _userService.FindUserByName(verifiedUser.Name);

            if (user != null) 
            {
                string hashedPassword = GetHashString(verifiedUser.Password, Convert.FromBase64String(user.Salt));

                if (!user.IsPasswordValid(hashedPassword)) return null;

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

        private static byte[] GetPseudorandomByteArray()
        {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        private static string GetHashString(string str, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: str,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 32
            ));
        }
    }
}
