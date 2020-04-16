using Ascalon.ClientService.Features.Exceptions;
using Ascalon.ClientService.Features.Users.Dtos;
using Ascalon.ClientService.Features.Users.GetUser;
using Ascalon.ClientService.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ascalon.ClientSerice.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ClientWebsite _clientWebsite;

        public AuthController(IMediator mediator, IOptions<ClientWebsite> clientWebsite)
        {
            _mediator = mediator;
            _clientWebsite = clientWebsite.Value;
        }

        [HttpPost()]
        public async Task<ActionResult> Login([FromBody]GetUserQuery query)
        {
            var user = await _mediator.Send(query);

            if (user == null)
                throw new NotFoundException();

            var identity = GetIdentity(user);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Json(new
            {
                access_token = encodedJwt,
                username = identity.Name,
                role = user.Role
            });
        }

        private ClaimsIdentity GetIdentity(User user)
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