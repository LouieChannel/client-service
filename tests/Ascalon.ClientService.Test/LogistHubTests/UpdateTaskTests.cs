using Ascalon.ClientService.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Ascalon.ClientService.Test.LogistHubTests
{
    public class UpdateTaskTests : SimpleServerFixture
    {
        public UpdateTaskTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task UpdateTaskTests_ReturnUpdatedTask()
        {
            try
            {
                var logistConnection = GetLogistConnection;

                var driverConnection = GetDriverConnection;

                driverConnection.On<string>(nameof(LogistHub.UpdateTask), orderItem =>
                {
                    Assert.NotEqual("[]", orderItem);
                });

                logistConnection.On<string>(nameof(LogistHub.UpdateTask), orderItem =>
                {
                    Assert.NotEqual("[]", orderItem);
                });

                await driverConnection.StartAsync();

                await logistConnection.StartAsync();

                await logistConnection.InvokeAsync(nameof(LogistHub.UpdateTask), $"{{\"Id\":1,\"Logist\":{{\"Id\":1, \"FullName\":\"Test\"}},\"Driver\":{{\"Id\":2, \"FullName\":\"Test\"}},\"Description\":\"Here is normal text\",\"StartLongitude\":50.1354,\"StartLatitude\":30.4324,\"EndLongitude\":1.4342,\"EndLatitude\":43.1234,\"Status\":2,\"Entity\":\"Normal text\",\"CreatedAt\":\"{DateTime.Now.ToString()}\"}}");

                await driverConnection.StopAsync();

                await logistConnection.StopAsync();
            }
            catch (Exception)
            {
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("test_update_task_test")]
        public async Task UpdateTaskTests_NotValidObject(string data)
        {
            try
            {
                var logistConnection = GetLogistConnection;

                var driverConnection = GetDriverConnection;

                driverConnection.On<string>(nameof(LogistHub.UpdateTask), orderItem =>
                {
                    Assert.NotEqual("[]", orderItem);
                });

                logistConnection.On<string>(nameof(LogistHub.UpdateTask), orderItem =>
                {
                    Assert.NotEqual("[]", orderItem);
                });

                await driverConnection.StartAsync();

                await logistConnection.StartAsync();

                await logistConnection.InvokeAsync(nameof(LogistHub.UpdateTask), data);

                await driverConnection.StopAsync();

                await logistConnection.StopAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}
