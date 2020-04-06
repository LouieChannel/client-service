using Ascalon.ClientService.Features.Exceptions;
using Ascalon.ClientService.Features.Tasks.Dtos;
using Ascalon.ClientService.Repositories;
using Ascalon.Uow;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Features.Tasks.GetDriverTask
{
    public class GetDriverTaskHandler : IRequestHandler<GetDriverTaskQuery, List<Tasks.Dtos.Task>>
    {
        private readonly TasksRepository _tasksRepository;
        private readonly UsersRepository _usersRepository;
        private readonly IMemoryCache _memoryCache;

        public GetDriverTaskHandler(IUnitOfWork uow, IMemoryCache memoryCache)
        {
            _tasksRepository = uow.GetRepository<TasksRepository>();
            _usersRepository = uow.GetRepository<UsersRepository>();
            _memoryCache = memoryCache;
        }

        public async Task<List<Tasks.Dtos.Task>> Handle(GetDriverTaskQuery request, CancellationToken cancellationToken)
        {
            return await _memoryCache.GetOrCreate(request.DriverId.ToString(), async options =>
            {
                options.AbsoluteExpiration = DateTime.Now.AddSeconds(1);

                var tasks = await _tasksRepository.GetTasksByDriverIdAsync(request.DriverId);

                if (tasks == null)
                    throw new NotFoundException();

                List<Dtos.Task> result = new List<Dtos.Task>();

                foreach (var task in tasks)
                {
                    task.Logist = await _usersRepository.GetUserByIdAsync(task.LogistId);
                    task.Driver = await _usersRepository.GetUserByIdAsync(task.DriverId);

                    result.Add(task.ToQueryTask());
                }

                return result;
            });
        }
    }
}
