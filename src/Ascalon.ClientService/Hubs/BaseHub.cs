using Ascalon.ClientService.Features.Users.Dtos;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

            var groupName = GetGroupName();

            if (string.IsNullOrEmpty(groupName))
                return Task.CompletedTask;

            Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var groupName = GetGroupName();

            if (string.IsNullOrEmpty(groupName))
                return Task.CompletedTask;

            Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            _logger.LogInformation($"Connection with id '{Context.ConnectionId}' was closed.");

            return base.OnDisconnectedAsync(exception);
        }

        private string GetGroupName()
        {
            var context = this.Context.GetHttpContext();

            var handler = new JwtSecurityTokenHandler();

            var jwt = string.Empty;

            if (context.Request.Query.ContainsKey("access_token"))
                jwt = context.Request.Query["access_token"];
            else
                jwt = context.Request.Headers.Where(i => i.Key == "Authorization").Select(i => i.Value.FirstOrDefault().Replace("Bearer ", "")).FirstOrDefault();

            var token = handler.ReadJwtToken(jwt);

            var role = token.Payload.Where(i => i.Key == "Role").Select(i => i.Value.ToString()).FirstOrDefault();

            if (string.IsNullOrEmpty(role))
                return null;

            foreach (var identity in context.User.Identities)
                identity.AddClaim(new Claim(role, "1"));

            if (role == RoleType.Driver.ToString())
            {
                var id = token.Payload.Where(i => i.Key == "Id").Select(i => i.Value.ToString()).FirstOrDefault();

                if (string.IsNullOrEmpty(id))
                    return null;

                return id;
            }
            else if (role == RoleType.Logist.ToString())
                return RoleType.Logist.ToString();

            return null;
        }
    }
}
