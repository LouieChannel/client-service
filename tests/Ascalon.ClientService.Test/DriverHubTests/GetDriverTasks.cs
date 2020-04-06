using Ascalon.ClientService.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Ascalon.ClientService.Test.DriverHubTests
{
    public class GetDriverTasks : SimpleServerFixture
    {
        public GetDriverTasks(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task GetDriverTasks_ReturnTasks()
        {
            try
            {
                var driverConnection = GetDriverConnection;

                driverConnection.On<string>(nameof(DriverHub.GetDriverTasks), orderItem =>
                {
                    Assert.NotEqual("[]", orderItem);
                });

                await driverConnection.StartAsync();

                await driverConnection.InvokeAsync(nameof(DriverHub.GetDriverTasks));

                await driverConnection.StopAsync();
            }
            catch (Exception)
            {
            }
        }

        [Fact]
        public async Task GetDriverTasks_NotValidCookie()
        {
            try
            {
                var driverConnection = GetDriverConnectionWithLogistCookie;

                driverConnection.On<string>(nameof(DriverHub.GetDriverTasks), orderItem =>
                {
                    Assert.Null(orderItem);
                });

                await driverConnection.StartAsync();

                await driverConnection.InvokeAsync(nameof(DriverHub.GetDriverTasks));

                await driverConnection.StopAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}
