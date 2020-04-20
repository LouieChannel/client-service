using Ascalon.ClientService.Features.Exceptions;
using Ascalon.ClientService.Features.Tasks.CreateTask.Dtos;
using Ascalon.ClientService.Repositories;
using Ascalon.Uow;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DtosTasks = Ascalon.ClientService.Features.Tasks.Dtos;

namespace Ascalon.ClientService.Features.Tasks.CreateTask
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, DtosTasks.Task>
    {
        private readonly TasksRepository _tasksRepository;
        private readonly UsersRepository _userRepository;

        public CreateTaskHandler(IUnitOfWork uow)
        {
            _tasksRepository = uow.GetRepository<TasksRepository>();
            _userRepository = uow.GetRepository<UsersRepository>();
        }

        public async Task<DtosTasks.Task> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _tasksRepository.CreateTaskAsync(request.ToEntityTask());

            if (task == null)
                throw new NotFoundException();

            task.Entity.Driver = await _userRepository.GetUserByIdAsync(task.Entity.DriverId);

            task.Entity.Logist = await _userRepository.GetUserByIdAsync(task.Entity.LogistId);

            return task.Entity.ToCommandTask();
        }
    }
}
