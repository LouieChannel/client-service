using Ascalon.ClientService.Features.Users.Dtos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ascalon.ClientService.Infrastructure
{
    public class AuthOptions
    {
        /// <summary>
        /// Возвращает или задаёт издатель токена.
        /// </summary>
        public const string ISSUER = "Ascalon.ClientService";

        /// <summary>
        /// Возвращает или задаёт потребитель токена.
        /// </summary>
        public const string AUDIENCE = "Ascalon.ClientWeb";

        /// <summary>
        /// Возвращает или задаёт ключ для шифрации.
        /// </summary>
        const string KEY = "ascalon.clientservice";

        public const int LIFETIME = 60000;

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }

        public static string GetJWT(User user)
        {
            var identity = GetIdentity(user);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: ISSUER,
                    audience: AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private static ClaimsIdentity GetIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Role", user.Role),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.FullName)
            };

            var claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
