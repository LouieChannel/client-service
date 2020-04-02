using Ascalon.ClientService.Features.Users.Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Hubs
{
    public abstract class BaseHub : Hub
    {
        private readonly ILogger _logger;

        public BaseHub(ILogger logger)
        {
            _logger = logger;
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"Connection with id '{Context.ConnectionId}' was opened.");

            Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName());

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName());

            _logger.LogInformation($"Connection with id '{Context.ConnectionId}' was closed.");

            return base.OnDisconnectedAsync(exception);
        }

        private string GetGroupName()
        {
            var context = this.Context.GetHttpContext();

            context.Request.Cookies.TryGetValue("Role", out string roleId);

            if (roleId == RoleType.Driver.ToString())
                return context.Request.Cookies["Id"];
            else if (roleId == RoleType.Logist.ToString())
                return context.Request.Cookies["Role"];

            return null;
        }
    }
}
