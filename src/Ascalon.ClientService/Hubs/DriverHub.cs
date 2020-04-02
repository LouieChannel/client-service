using Ascalon.ClientService.Features.Exceptions;
using Ascalon.ClientService.Repositories;
using Ascalon.Uow;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Hubs
{
    public class DriverHub : BaseHub
    {
        private readonly ILogger<DriverHub> _logger;
        private readonly IMemoryCache _cache;
        private readonly TasksRepository _tasksRepository;
        private readonly IServiceProvider _serviceProvider;

        public DriverHub(
            IServiceProvider serviceProvider,
            ILogger<DriverHub> logger,
            IUnitOfWork uow,
            IMemoryCache cache) : base(logger)
        {
            _serviceProvider = serviceProvider;
            _cache = cache;
            _tasksRepository = uow.GetRepository<TasksRepository>();
            _logger = logger;
        }

        public async Task UpdateStatus(int taskId, short statusId)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                _logger.LogInformation($"{nameof(DriverHub)} with connectionId '{Context.ConnectionId}' received message in method '{nameof(UpdateStatus)}'");

                var task = await _tasksRepository.GetTaskByIdAsync(taskId);

                if (task == null)
                    throw new NotFoundException();

                task.Status = statusId;

                await Clients.Groups(new List<string>() { "Logist", task.DriverId.ToString() }).SendAsync(nameof(UpdateStatus),
                    (await _tasksRepository.UpdateTaskAsync(task)).Entity);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Was occured error in method {nameof(UpdateStatus)}.", exception);
            }
        }
    }
}
