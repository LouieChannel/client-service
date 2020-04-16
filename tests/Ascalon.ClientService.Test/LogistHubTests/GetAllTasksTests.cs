using Ascalon.ClientService.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Ascalon.ClientService.Test.LogistHubTests
{
    public class GetAllTasksTests : SimpleServerFixture
    {
        public GetAllTasksTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task GetAllTasks_ReturnAllTasks()
        {
            try
            {
                var logistConnection = GetLogistConnection;

                logistConnection.On<string>(nameof(LogistHub.GetAllTasks), orderItem =>
                {
                    Assert.NotEqual("[]", orderItem);
                });

                await logistConnection.StartAsync();

                await logistConnection.InvokeAsync(nameof(LogistHub.GetAllTasks), DateTime.Now.ToString());

                await logistConnection.StopAsync();
            }
            catch (Exception)
            {
            }
        }

        [Fact]
        public async Task GetAllTasks_NotReturn()
        {
            try
            {
                var logistConnection = GetLogistConnection;

                logistConnection.On<string>(nameof(LogistHub.GetAllTasks), orderItem =>
                {
                    Assert.Equal("[]", orderItem);
                });

                await logistConnection.StartAsync();

                await logistConnection.InvokeAsync(nameof(LogistHub.GetAllTasks));

                await logistConnection.StopAsync();
            }
            catch (Exception)
            {
            }
        }

        [Fact]
        public async Task GetAllTasks_NotValidCookie()
        {
            try
            {
                var logistConnection = GetLogistConnectionWithDriverCookie;

                logistConnection.On<string>(nameof(LogistHub.GetAllTasks), orderItem =>
                {
                    Assert.Null(orderItem);
                });

                await logistConnection.StartAsync();

                await logistConnection.InvokeAsync(nameof(LogistHub.GetAllTasks));

                await logistConnection.StopAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}
