using Ascalon.ClientService.Features.Exceptions;
using Ascalon.ClientService.Features.Tasks.Dtos;
using Ascalon.ClientService.Repositories;
using Ascalon.Uow;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Features.Tasks.GetTask
{
    public class GetTaskHandler : IRequestHandler<GetTaskQuery, Tasks.Dtos.Task>
    {
        private readonly TasksRepository _tasksRepository;
        private readonly IMemoryCache _memoryCache;

        public GetTaskHandler(IUnitOfWork uow, IMemoryCache memoryCache)
        {
            _tasksRepository = uow.GetRepository<TasksRepository>();
            _memoryCache = memoryCache;
        }

        public async Task<Tasks.Dtos.Task> Handle(GetTaskQuery request, CancellationToken cancellationToken)
        {
            return await _memoryCache.GetOrCreate(request.Id.ToString(), async options =>
            {
                var task = await _tasksRepository.GetTaskByIdAsync(request.Id);

                if (task == null)
                    throw new NotFoundException();

                return task.ToQueryTask();
            });

        }
    }
}
