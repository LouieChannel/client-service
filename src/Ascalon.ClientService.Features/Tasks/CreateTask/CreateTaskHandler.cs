using DtosTasks = Ascalon.ClientService.Features.Tasks.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Ascalon.Uow;
using Microsoft.Extensions.Caching.Memory;
using Ascalon.ClientService.Repositories;
using System.Threading.Tasks;
using System.Threading;
using Ascalon.ClientService.Features.Tasks.CreateTask.Dtos;

namespace Ascalon.ClientService.Features.Tasks.CreateTask
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, Unit>
    {
        private readonly TasksRepository _tasksRepository;

        public CreateTaskHandler(IUnitOfWork uow)
        {
            _tasksRepository = uow.GetRepository<TasksRepository>();
        }

        public async Task<Unit> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            if (await _tasksRepository.CreateTaskAsync(request.ToEntityTask()) == null)
                throw new Exception();

            return Unit.Value;
        }
    }
}
