using Ascalon.ClientService.Features.Tasks.UpdateTask.Dtos;
using Ascalon.ClientService.Repositories;
using Ascalon.Uow;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Features.Tasks.UpdateTask
{
    public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, Unit>
    {
        private readonly TasksRepository _tasksRepository;

        public UpdateTaskHandler(IUnitOfWork uow)
        {
            _tasksRepository = uow.GetRepository<TasksRepository>();
        }

        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var updatedTask = await _tasksRepository.UpdateTaskAsync(request.ToDataTask());

            if (updatedTask == null)
                throw new Exception();

            return Unit.Value;
        }
    }
}
