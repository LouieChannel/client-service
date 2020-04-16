using Ascalon.ClientService.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Ascalon.ClientService.Test.LogistHubTests
{
    public class CreateTaskTests : SimpleServerFixture
    {
        public CreateTaskTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task CreateTask_ReturnCreatedTask()
        {
            var logistConnection = GetLogistConnection;

            var driverConnection = GetDriverConnection;

            driverConnection.On<string>(nameof(LogistHub.CreateTask), orderItem =>
            {
                Assert.NotEqual("[]", orderItem);
            });

            logistConnection.On<string>(nameof(LogistHub.CreateTask), orderItem =>
            {
                Assert.NotEqual("[]", orderItem);
            });

            await driverConnection.StartAsync();

            await logistConnection.StartAsync();

            await logistConnection.InvokeAsync(nameof(LogistHub.CreateTask), "{\"Driver\":{\"Id\":2, \"FullName\":\"Test\"},\"Description\":\"test\",\"StartLongitude\":50.1354,\"StartLatitude\":30.4324,\"EndLongitude\":1.4342,\"EndLatitude\":43.1234,\"Status\":1,\"Entity\":\"dasfas\"}");

            await driverConnection.StopAsync();

            await logistConnection.StopAsync();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("test_data_create_task")]
        public async Task CreateTask_NotValidObject(string data)
        {
            try
            {
                var logistConnection = GetLogistConnection;

                var driverConnection = GetDriverConnection;

                driverConnection.On<string>(nameof(LogistHub.CreateTask), orderItem =>
                {
                    Assert.NotEqual("[]", orderItem);
                });

                logistConnection.On<string>(nameof(LogistHub.CreateTask), orderItem =>
                {
                    Assert.NotEqual("[]", orderItem);
                });

                await driverConnection.StartAsync();

                await logistConnection.StartAsync();

                await logistConnection.InvokeAsync(nameof(LogistHub.CreateTask), data);

                await driverConnection.StopAsync();

                await logistConnection.StopAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}
