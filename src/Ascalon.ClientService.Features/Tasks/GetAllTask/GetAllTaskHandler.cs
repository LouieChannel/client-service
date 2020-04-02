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

namespace Ascalon.ClientService.Features.Tasks.GetAllTask
{
    public class GetAllTaskHandler : IRequestHandler<GetAllTaskQuery, List<Tasks.Dtos.Task>>
    {
        private readonly TasksRepository _tasksRepository;
        private readonly IMemoryCache _memoryCache;

        public GetAllTaskHandler(IUnitOfWork uow, IMemoryCache memoryCache)
        {
            _tasksRepository = uow.GetRepository<TasksRepository>();
            _memoryCache = memoryCache;
        }

        public async Task<List<Tasks.Dtos.Task>> Handle(GetAllTaskQuery request, CancellationToken cancellationToken)
        {
            return await _memoryCache.GetOrCreate(request.DateFilter.ToString(), async options =>
            {
                options.AbsoluteExpiration = DateTime.Now.AddSeconds(1);

                var task = await _tasksRepository.GetTasks(request.DateFilter);

                if (task == null)
                    throw new NotFoundException();

                return task.Select(i => i.ToQueryTask()).ToList();
            });
        }
    }
}
