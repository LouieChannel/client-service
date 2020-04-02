using Ascalon.ClientService.Features.Exceptions;
using Ascalon.ClientService.Features.Tasks.Dtos;
using Ascalon.ClientService.Repositories;
using Ascalon.Uow;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Features.Tasks.GetDriverTask
{
    public class GetDriverTaskHandler : IRequestHandler<GetDriverTaskQuery, List<Tasks.Dtos.Task>>
    {
        private readonly TasksRepository _tasksRepository;
        private readonly IMemoryCache _memoryCache;

        public GetDriverTaskHandler(IUnitOfWork uow, IMemoryCache memoryCache)
        {
            _tasksRepository = uow.GetRepository<TasksRepository>();
            _memoryCache = memoryCache;
        }

        public async Task<List<Tasks.Dtos.Task>> Handle(GetDriverTaskQuery request, CancellationToken cancellationToken)
        {
            return await _memoryCache.GetOrCreate(request.DriverId.ToString(), async options =>
            {
                options.AbsoluteExpiration = DateTime.Now.AddSeconds(1);

                var task = (await _tasksRepository.GetTasksByDriverIdAsync(request.DriverId));

                if (task == null)
                    throw new NotFoundException();

                return task.Select(i => i.ToQueryTask()).ToList();
            });
        }
    }
}
