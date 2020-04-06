using Ascalon.ClientService.Features.Tasks.Dtos;
using Ascalon.ClientService.Features.Tasks.GetDriverTask;
using Ascalon.ClientService.Features.Tasks.UpdateTask;
using Ascalon.ClientService.Features.Users.Dtos;
using Ascalon.ClientService.Infrastructure;
using Ascalon.ClientService.Kafka;
using Ascalon.ClientService.Kafka.Services;
using Ascalon.ClientService.Repositories;
using Ascalon.Uow;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Task = System.Threading.Tasks.Task;

namespace Ascalon.ClientService.Hubs
{
    public class DriverHub : BaseHub
    {
        private readonly ILogger<DriverHub> _logger;
        private readonly IMemoryCache _cache;
        private readonly TasksRepository _tasksRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<LogistHub> _logistHub;

        public DriverHub(
            IServiceProvider serviceProvider,
            ILogger<DriverHub> logger,
            IUnitOfWork uow,
            IMemoryCache cache,
            IHubContext<LogistHub> logistHub) : base(logger)
        {
            _serviceProvider = serviceProvider;
            _cache = cache;
            _tasksRepository = uow.GetRepository<TasksRepository>();
            _logger = logger;
            _logistHub = logistHub;
        }

        public async Task GetDriverTasks()
        {
            try
            {
                if (CheckRole())
                    return;

                Context.GetHttpContext().Request.Cookies.TryGetValue("Id", out string userId);

                if (string.IsNullOrEmpty(userId))
                    return;

                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                _logger.LogInformation($"{nameof(DriverHub)} with connectionId '{Context.ConnectionId}' received message in method '{nameof(GetDriverTasks)}'");

                var tasks = (await mediator.Send(new GetDriverTaskQuery()
                {
                    DriverId = Convert.ToInt32(userId),
                })).ToJson();

                await Clients.Caller.SendAsync(nameof(GetDriverTasks), tasks);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Was occured error in method {nameof(GetDriverTasks)}.", exception);
            }
        }

        public async Task UpdateStatus(string updateStatusCommand)
        {
            try
            {
                if (CheckRole())
                    return;

                if (string.IsNullOrEmpty(updateStatusCommand))
                    return;

                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var request = updateStatusCommand.FromJson<UpdateTaskCommand>();

                _logger.LogInformation($"{nameof(DriverHub)} with connectionId '{Context.ConnectionId}' received message in method '{nameof(UpdateStatus)}'");

                var task = await mediator.Send(request);

                if (request.Status == StatusType.InProgress)
                {
                    DumperStateConsumerService.DriversTasks.TryAdd(task.Driver.DumperId.Value, task.Id);

                    DumperStateConsumerService.NotificationTasks.TryGetValue(task.Driver.DumperId.Value, out int predict);

                    if (predict != 0)
                        await _logistHub.Clients.Group("Logist").SendAsync("DumperStatus", new DumperStatus()
                        {
                            Id = task.Id,
                            State = predict
                        }.ToJson());
                }
                else if (request.Status == StatusType.Done || request.Status == StatusType.Cancelled)
                {
                    DumperStateConsumerService.DriversTasks.TryRemove(task.Driver.DumperId.Value, out int taskId);
                }

                var result = task.ToJson();

                await Clients.Group(task.Driver.Id.ToString()).SendAsync(nameof(UpdateStatus), result);

                await _logistHub.Clients.Group("Logist").SendAsync(nameof(UpdateStatus), result);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Was occured error in method {nameof(UpdateStatus)}.", exception);
            }
        }

        private bool CheckRole()
        {
            Context.GetHttpContext().Request.Cookies.TryGetValue("Role", out string roleId);

            return string.IsNullOrEmpty(roleId) || (roleId == RoleType.Logist.ToString());
        }
    }
}
