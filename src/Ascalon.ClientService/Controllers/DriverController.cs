using Ascalon.ClientService.Features.Exceptions;
using Ascalon.ClientService.Features.Tasks.GetDriverTask;
using Ascalon.ClientService.Features.Users.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Controllers
{
    [Route("[controller]")]
    public class DriverController : Controller
    {
        private readonly IMediator _mediator;

        public DriverController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetDriverTask()
        {
            HttpContext.Request.Cookies.TryGetValue("Role", out string roleId);

            if (string.IsNullOrEmpty(roleId))
                throw new ForbiddenException("Проведите авторизацию пользователя.");

            if (roleId == RoleType.Logist.ToString())
                throw new ForbiddenException("Данный пользователь не может получить доступ к данной странице.");

            HttpContext.Request.Cookies.TryGetValue("Id", out string Id);

            if (string.IsNullOrEmpty(Id))
                throw new NotFoundException();

            return Ok(await _mediator.Send(new GetDriverTaskQuery()
            {
                DriverId = Convert.ToInt32(Id),
            }));
        }
    }
}