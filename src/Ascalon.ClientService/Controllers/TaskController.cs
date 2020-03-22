using Ascalon.ClientService.Features.Tasks.GetTask;
using MediatR;
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

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById([FromRoute]int id) => Ok(await _mediator.Send(new GetTaskQuery()
        {
            Id = id,
        }));
    }
}