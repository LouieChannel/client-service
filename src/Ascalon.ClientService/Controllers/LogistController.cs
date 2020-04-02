using Ascalon.ClientService.Features.Exceptions;
using Ascalon.ClientService.Features.Tasks.GetAllTask;
using Ascalon.ClientService.Features.Users.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Controllers
{
    [Route("[controller]")]
    public class LogistController : Controller
    {
        private readonly IMediator _mediator;

        public LogistController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllTask()
        {
            HttpContext.Request.Cookies.TryGetValue("Role", out string roleId);

            if (string.IsNullOrEmpty(roleId))
                throw new ForbiddenException("Проведите авторизацию пользователя.");

            if (roleId == RoleType.Driver.ToString())
                throw new ForbiddenException("Данный пользователь не может получить доступ к данной странице.");

            return Ok(await _mediator.Send(new GetAllTaskQuery()
            {
                DateFilter = DateTime.Now,
            }));
        }
    }
}