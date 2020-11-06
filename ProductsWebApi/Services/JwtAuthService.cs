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
using ProductsWebApi.Models;
using ProductsWebApi.Models.Extensions;
using ProductsWebApi.Models.Json;

namespace ProductsWebApi.Services
{
    public class JwtAuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly AuthOptions _token;

        public JwtAuthService(IOptions<ApplicationSettings> appSettings, IUserService userService)
        {
            _userService = userService;
            _token = appSettings.Value.AuthToken;
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

        public async Task AddUser(User user)
        {
            byte[] salt = GetPseudorandomByteArray();
            string password = GetHashString(user.Password, salt);

            await _userService.Add(new Models.Entities.UserEntity {
                Name = user.Username,
                Password = password,
                Salt = Convert.ToBase64String(salt),
                Role = "admin"
            });
        }

        private ClaimsIdentity GetIdentity(User verifiedUser)
        {
            var user = _userService.FindUserByName(verifiedUser.Username);

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

        private byte[] GetPseudorandomByteArray()
        {
            var salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        private string GetHashString(string str, byte[] salt)
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
