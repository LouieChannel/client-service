using Ascalon.ClientService.Features.Tasks.CreateTask;
using Ascalon.ClientService.Features.Tasks.GetTask.Dtos;
using Ascalon.ClientService.Repositories;
using Ascalon.Uow;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Hubs
{
    public class LogistHub : BaseHub
    {
        private readonly ILogger<LogistHub> _logger;
        private readonly IMemoryCache _cache;
        private readonly TasksRepository _tasksRepository;
        private readonly IServiceProvider _serviceProvider;

        public LogistHub(
            IServiceProvider serviceProvider,
            ILogger<LogistHub> logger,
            IUnitOfWork uow,
            IMemoryCache cache) : base(logger)
        {
            _serviceProvider = serviceProvider;
            _cache = cache;
            _tasksRepository = uow.GetRepository<TasksRepository>();
            _logger = logger;
        }

        public async Task CreateTask(string createTaskCommand)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var request = JsonConvert.DeserializeObject<CreateTaskCommand>(createTaskCommand);

                _logger.LogInformation($"{nameof(LogistHub)} with connectionId '{Context.ConnectionId}' received message in method '{nameof(CreateTask)}'");

                var task = await mediator.Send(request);

                await Clients.Groups(new List<string>() { "Logist", request.DriverId.ToString()}).SendAsync(nameof(CreateTask), 
                    (await _tasksRepository.GetTaskAsync(task.DriverId, (short)task.Status, task.CreatedAt)).ToQueryTask());
            }
            catch (Exception exception)
            {
                _logger.LogError($"Was occured error in method {nameof(CreateTask)}.", exception);
            }
        }
    }
}
