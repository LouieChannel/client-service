using Ascalon.ClientService.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Ascalon.ClientService.Test.LogistHubTests
{
    public class GetAllDriversTests : SimpleServerFixture
    {
        public GetAllDriversTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task GetAllDrivers_ReturnAllDrivers()
        {
            try
            {
                var logistConnection = GetLogistConnection;

                logistConnection.On<string>(nameof(LogistHub.GetAllDrivers), orderItem =>
                {
                    Assert.NotEqual("[]", orderItem);
                });

                await logistConnection.StartAsync();

                await logistConnection.InvokeAsync(nameof(LogistHub.GetAllDrivers));

                await logistConnection.StopAsync();
            }
            catch (Exception)
            {
            }
        }

        [Fact]
        public async Task GetAllDrivers_NotReturn()
        {
            try
            {
                var logistConnection = GetLogistConnectionWithDriverCookie;

                logistConnection.On<string>(nameof(LogistHub.GetAllDrivers), orderItem =>
                {
                    Assert.Equal("[]", orderItem);
                });

                await logistConnection.StartAsync();

                await logistConnection.InvokeAsync(nameof(LogistHub.GetAllDrivers));

                await Assert.ThrowsAsync<Exception>(() => logistConnection.StopAsync());
            }
            catch (Exception ex)
            {
                Assert.Equal("Failed to invoke 'GetAllDrivers' because user is unauthorized", ex.Message);
            }
        }
    }
}
