﻿using Ascalon.ClientService.Features.Exceptions;
using Ascalon.ClientService.Features.Users.GetUser;
using Ascalon.ClientService.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ascalon.ClientSerice.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]
        public async Task<ActionResult> Login([FromBody]GetUserQuery query)
        {
            var user = await _mediator.Send(query);

            if (user == null)
                throw new NotFoundException();

            var jwt = AuthOptions.GetJWT(user);

            return Json(new
            {
                access_token = jwt,
                username = user.FullName,
                role = user.Role
            });
        }


    }
}