using Ascalon.ClientService.Features.Tasks.CreateTask;
using Ascalon.ClientService.Features.Tasks.GetAllTask;
using Ascalon.ClientService.Features.Tasks.UpdateTask;
using Ascalon.ClientService.Infrastructure;
using Ascalon.ClientService.Repositories;
using Ascalon.Uow;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using ThreadTask = System.Threading.Tasks.Task;

namespace Ascalon.ClientService.Hubs
{
    public class LogistHub : BaseHub
    {
        private readonly ILogger<LogistHub> _logger;
        private readonly IMemoryCache _cache;
        private readonly TasksRepository _tasksRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<DriverHub> _driverHub;


        public LogistHub(
            IServiceProvider serviceProvider,
            ILogger<LogistHub> logger,
            IUnitOfWork uow,
            IMemoryCache cache,
            IHubContext<DriverHub> driverHub) : base(logger)
        {
            _serviceProvider = serviceProvider;
            _cache = cache;
            _tasksRepository = uow.GetRepository<TasksRepository>();
            _logger = logger;
            _driverHub = driverHub;
        }

        [Authorize(Policy = "Logist")]
        public async ThreadTask GetAllTasks()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                _logger.LogInformation($"{nameof(LogistHub)} with connectionId '{Context.ConnectionId}' received message in method '{nameof(GetAllTasks)}'");

                var tasks = (await mediator.Send(new GetAllTaskQuery()
                {
                    DateFilter = DateTime.Now,
                })).ToJson();

                await Clients.Caller.SendAsync(nameof(GetAllTasks), tasks);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Was occured error in method {nameof(GetAllTasks)}.", exception);
            }
        }

        [Authorize(Policy = "Logist")]
        public async ThreadTask CreateTask(string createTaskCommand)
        {
            try
            {
                if (string.IsNullOrEmpty(createTaskCommand))
                    return;

                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var request = createTaskCommand.FromJson<CreateTaskCommand>();

                request.Logist = null;

                _logger.LogInformation($"{nameof(LogistHub)} with connectionId '{Context.ConnectionId}' received message in method '{nameof(CreateTask)}'");

                var task = (await mediator.Send(request)).ToJson();

                await Clients.Group("Logist").SendAsync(nameof(CreateTask), task);

                await _driverHub.Clients.Group(request.Driver.Id.ToString()).SendAsync(nameof(CreateTask), task);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Was occured error in method {nameof(CreateTask)}.", exception);
            }
        }

        [Authorize(Policy = "Logist")]
        public async ThreadTask UpdateTask(string updateStatusCommand)
        {
            try
            {
                if (string.IsNullOrEmpty(updateStatusCommand))
                    return;

                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var request = updateStatusCommand.FromJson<UpdateTaskCommand>();

                _logger.LogInformation($"{nameof(LogistHub)} with connectionId '{Context.ConnectionId}' received message in method '{nameof(UpdateTask)}'");

                var result = (await mediator.Send(request)).ToJson();

                await Clients.Group("Logist").SendAsync(nameof(UpdateTask), result);

                await _driverHub.Clients.Group(request.Driver.Id.ToString()).SendAsync(nameof(UpdateTask), result);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Was occured error in method {nameof(UpdateTask)}.", exception);
            }
        }
    }
}
