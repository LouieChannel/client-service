using Ascalon.ClientService.Features.Tasks.CreateTask.Dtos;
using Ascalon.ClientService.Repositories;
using Ascalon.Uow;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DtosTasks = Ascalon.ClientService.Features.Tasks.Dtos;

namespace Ascalon.ClientService.Features.Tasks.CreateTask
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, DtosTasks.Task>
    {
        private readonly TasksRepository _tasksRepository;

        public CreateTaskHandler(IUnitOfWork uow)
        {
            _tasksRepository = uow.GetRepository<TasksRepository>();
        }

        public async Task<DtosTasks.Task> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var result = await _tasksRepository.CreateTaskAsync(request.ToEntityTask());

            if (result == null)
                throw new Exception();

            return result.ToCommandTask();
        }
    }
}
