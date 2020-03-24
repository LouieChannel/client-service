using Ascalon.ClientService.Features.Tasks.CreateTask;
using Ascalon.ClientService.Features.Tasks.GetTask;
using Ascalon.ClientService.Features.Tasks.UpdateTask;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Controllers
{
    [Route("[controller]")]
    public class TaskController : Controller
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody]CreateTaskCommand createTaskCommand) => Created("task", await _mediator.Send(createTaskCommand));

        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Update([FromRoute]int id, [FromBody]UpdateTaskCommand updateTaskCommand)
        {
            updateTaskCommand.Id = id;

            return Ok(await _mediator.Send(updateTaskCommand));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetById([FromRoute]int id) => Ok(await _mediator.Send(new GetTaskQuery()
        {
            Id = id,
        }));
    }
}