using Ascalon.ClientService.Features.Users.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Hubs
{
    public abstract class BaseHub : Hub
    {
        private readonly ILogger _logger;
        private static HttpClient _httpClient;

        public BaseHub(ILogger logger, IHttpClientFactory clientFactory)
        {
            _logger = logger;

            _httpClient = clientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("http://35.228.72.226");
        }

        public override Task OnConnectedAsync()
        {
            _logger.LogInformation($"Connection with id '{Context.ConnectionId}' was opened.");

            var groupName = GetGroupName();

            if (string.IsNullOrEmpty(groupName))
                return Task.CompletedTask;

            Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            if (TryGetId(groupName, out string Id))
                _httpClient.GetAsync($"/shift/start/{Id}");

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var groupName = GetGroupName();

            if (string.IsNullOrEmpty(groupName))
                return Task.CompletedTask;

            Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            _logger.LogInformation($"Connection with id '{Context.ConnectionId}' was closed.");

            if (TryGetId(groupName, out string Id))
                _httpClient.GetAsync($"/shift/stop/{Id}");

            return base.OnDisconnectedAsync(exception);
        }

        private bool TryGetId(string roleType, out string id)
        {
            id = null;

            if (roleType == RoleType.Logist.ToString())
                return false;

            id = roleType;

            return true;
        }

        private string GetGroupName()
        {
            var context = Context.GetHttpContext();

            var token = GetJwtSecurityToken(context);

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

        private JwtSecurityToken GetJwtSecurityToken(HttpContext context)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = string.Empty;

            if (context.Request.Query.ContainsKey("access_token"))
                jwt = context.Request.Query["access_token"];
            else
                jwt = context.Request.Headers.Where(i => i.Key == "Authorization").Select(i => i.Value.FirstOrDefault().Replace("Bearer ", "")).FirstOrDefault();

            return handler.ReadJwtToken(jwt);
        }
    }
}
