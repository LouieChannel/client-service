using Ascalon.ClientService.Features.Exceptions;
using Ascalon.ClientService.Features.Users.CreateUser;
using Ascalon.ClientService.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<ActionResult> Login([FromBody]CreateUserCommand command)
        {
            var user = await _mediator.Send(command);

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