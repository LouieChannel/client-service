﻿using Ascalon.ClientService.Features.Tasks.Dtos;
using Ascalon.ClientService.Features.Tasks.GetDriverTask;
using Ascalon.ClientService.Features.Tasks.UpdateTask;
using Ascalon.ClientService.Infrastructure;
using Ascalon.ClientService.Kafka;
using Ascalon.ClientService.Repositories;
using Ascalon.Uow;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using Task = System.Threading.Tasks.Task;

namespace Ascalon.ClientService.Hubs
{
    public class DriverHub : BaseHub
    {
        private readonly ILogger<DriverHub> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly TasksRepository _tasksRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<LogistHub> _logistHub;

        public DriverHub(
            IServiceProvider serviceProvider,
            ILogger<DriverHub> logger,
            IUnitOfWork uow,
            IMemoryCache cache,
            IHttpClientFactory clientFactory,
            IHubContext<LogistHub> logistHub) : base(logger, clientFactory)
        {
            _serviceProvider = serviceProvider;
            _memoryCache = cache;
            _tasksRepository = uow.GetRepository<TasksRepository>();
            _logger = logger;
            _logistHub = logistHub;
        }

        [Authorize(Policy = "Driver")]
        public async Task GetDriverTasks()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                _logger.LogInformation($"{nameof(DriverHub)} with connectionId '{Context.ConnectionId}' received message in method '{nameof(GetDriverTasks)}'");

                var tasks = (await mediator.Send(new GetDriverTaskQuery()
                {
                    DriverId = GetDriverId(),
                })).ToJson();

                await Clients.Caller.SendAsync(nameof(GetDriverTasks), tasks);
            }
            catch (Exception exception)
            {
                _logger.LogError($"Was occured error in method {nameof(GetDriverTasks)}.", exception);
            }
        }

        [Authorize(Policy = "Driver")]
        public async Task UpdateStatus(string updateStatusCommand)
        {
            try
            {
                if (string.IsNullOrEmpty(updateStatusCommand))
                    return;

                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var request = updateStatusCommand.FromJson<UpdateTaskCommand>();

                _logger.LogInformation($"{nameof(DriverHub)} with connectionId '{Context.ConnectionId}' received message in method '{nameof(UpdateStatus)}'");

                var task = await mediator.Send(request);

                var driverTasks = _memoryCache.GetOrCreate("DriversTasks", options =>
                {
                    return new ConcurrentDictionary<int, int>();
                });

                if (request.Status == StatusType.InProgress)
                {
                    var notificationTasks = _memoryCache.GetOrCreate("NotificationTasks", options =>
                    {
                        return new ConcurrentDictionary<int, int>();
                    });

                    driverTasks.TryAdd(task.Driver.Id, task.Id);

                    notificationTasks.TryGetValue(task.Driver.Id, out int predict);

                    if (predict != 0)
                        await _logistHub.Clients.Group("Logist").SendAsync("DumperStatus", new DumperStatus()
                        {
                            Id = task.Id,
                            State = predict
                        }.ToJson());
                }
                else if (request.Status == StatusType.Done || request.Status == StatusType.Cancelled)
                {
                    driverTasks.TryRemove(task.Driver.Id, out int taskId);
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

        private int GetDriverId()
        {
            var context = Context.GetHttpContext();

            var handler = new JwtSecurityTokenHandler();


            var jwt = string.Empty;

            if (context.Request.Query.ContainsKey("access_token"))
                jwt = context.Request.Query["access_token"];
            else
                jwt = context.Request.Headers.Where(i => i.Key == "Authorization").Select(i => i.Value.FirstOrDefault().Replace("Bearer ", "")).FirstOrDefault();

            var token = handler.ReadJwtToken(jwt);

            var role = token.Payload.Where(i => i.Key == "Id").Select(i => i.Value.ToString()).FirstOrDefault();

            if (string.IsNullOrEmpty(role))
                return 0;

            return Convert.ToInt32(role);
        }
    }
}
