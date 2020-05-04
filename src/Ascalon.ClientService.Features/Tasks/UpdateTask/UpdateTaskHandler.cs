using Ascalon.ClientService.Features.Exceptions;
using Ascalon.ClientService.Features.Tasks.UpdateTask.Dtos;
using Ascalon.ClientService.Repositories;
using Ascalon.Uow;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Features.Tasks.UpdateTask
{
    public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, Tasks.Dtos.Task>
    {
        private readonly TasksRepository _tasksRepository;
        private readonly UsersRepository _usersRepository;

        public UpdateTaskHandler(IUnitOfWork uow)
        {
            _tasksRepository = uow.GetRepository<TasksRepository>();
            _usersRepository = uow.GetRepository<UsersRepository>();
        }

        public async Task<Tasks.Dtos.Task> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var updatedTask =  _tasksRepository.UpdateTaskAsync(request.ToDataTask());

            if (updatedTask == null)
                throw new NotFoundException();

            updatedTask.Entity.Driver = await _usersRepository.GetUserByIdAsync(updatedTask.Entity.DriverId);
            updatedTask.Entity.Logist = await _usersRepository.GetUserByIdAsync(updatedTask.Entity.LogistId);

            return updatedTask.Entity.ToCommandTask();
        }
    }
}
