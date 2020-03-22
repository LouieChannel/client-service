using Ascalon.ClientService.Features.Users.GetUser;
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

        [HttpGet("{login}-{password}")]
        public async Task<ActionResult> Login([FromRoute]string login, [FromRoute]string password)
        {
            var user = await _mediator.Send(new GetUserQuery()
            {
                Login = login,
                Password = password
            });

            Response.Cookies.Append("Role", user.Role);
            Response.Cookies.Append("Name", user.FullName);

            return new RedirectResult($"{Request.Scheme}://{Request.Host}{Request.Path}", true);
        }

        [HttpGet("logout")]
        public ActionResult Logout()
        {
            Response.Cookies.Delete("Role");
            Response.Cookies.Delete("Name");

            return new RedirectResult($"{Request.Scheme}://{Request.Host}{Request.Path}", true);
        }
    }
}