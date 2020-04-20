using Ascalon.ClientService.Features.Tasks.CreateTask;
using Ascalon.ClientService.Features.Tasks.GetAllTask;
using Ascalon.ClientService.Features.Tasks.UpdateTask;
using Ascalon.ClientService.Features.Users.GetDrivers;
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
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using ThreadTask = System.Threading.Tasks.Task;

namespace Ascalon.ClientService.Hubs
{
    public class LogistHub : BaseHub
    {
        private readonly ILogger<LogistHub> _logger;
        private readonly IMemoryCache _cache;
        private readonly TasksRepository _tasksRepository;
        private readonly UsersRepository _usersRepository;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<DriverHub> _driverHub;


        public LogistHub(
            IServiceProvider serviceProvider,
            ILogger<LogistHub> logger,
            IUnitOfWork uow,
            IMemoryCache cache,
            IHttpClientFactory clientFactory,
            IHubContext<DriverHub> driverHub) : base(logger, clientFactory)
        {
            _serviceProvider = serviceProvider;
            _cache = cache;
            _tasksRepository = uow.GetRepository<TasksRepository>();
            _usersRepository = uow.GetRepository<UsersRepository>();
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

                request.Logist = GetLogist();

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
        public async ThreadTask GetAllDrivers()
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var drivers = (await mediator.Send(new GetDriversQuery())).ToJson();

                if (string.IsNullOrEmpty(drivers))
                    return;

                _logger.LogInformation($"{nameof(LogistHub)} with connectionId '{Context.ConnectionId}' received message in method '{nameof(CreateTask)}'");

                await Clients.Caller.SendAsync(nameof(GetAllDrivers), drivers);
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

        private Features.Tasks.Dtos.User GetLogist()
        {
            var context = Context.GetHttpContext();

            var handler = new JwtSecurityTokenHandler();

            var jwt = string.Empty;

            if (context.Request.Query.ContainsKey("access_token"))
                jwt = context.Request.Query["access_token"];
            else
                jwt = context.Request.Headers.Where(i => i.Key == "Authorization").Select(i => i.Value.FirstOrDefault().Replace("Bearer ", "")).FirstOrDefault();

            var token = handler.ReadJwtToken(jwt);

            var id = token.Payload.Where(i => i.Key == "Id").Select(i => i.Value.ToString()).FirstOrDefault();
            var fullName = token.Payload.Where(i => i.Key == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Select(i => i.Value.ToString()).FirstOrDefault();

            if (string.IsNullOrEmpty(id))
                return null;

            return new Features.Tasks.Dtos.User()
            {
                Id = Int32.Parse(id),
                FullName = fullName
            };
        }
    }
}
