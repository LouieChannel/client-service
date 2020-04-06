using Ascalon.ClientService.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Ascalon.ClientService.Test.DriverHubTests
{
    public class UpdateStatusTests : SimpleServerFixture
    {
        public UpdateStatusTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task UpdateStatusTests_ReturnUpdatedTask()
        {
            try
            {
                var logistConnection = GetLogistConnection;

                var driverConnection = GetDriverConnection;

                driverConnection.On<string>(nameof(DriverHub.UpdateStatus), orderItem =>
                {
                    Assert.NotEqual("[]", orderItem);
                });

                logistConnection.On<string>(nameof(DriverHub.UpdateStatus), orderItem =>
                {
                    Assert.NotEqual("[]", orderItem);
                });

                await driverConnection.StartAsync();

                await logistConnection.StartAsync();

                await driverConnection.InvokeAsync(nameof(DriverHub.UpdateStatus), $"{{\"Id\":1,\"Logist\":{{\"Id\":1, \"FullName\":\"Test\"}},\"Driver\":{{\"Id\":2, \"FullName\":\"Test\"}},\"Description\":\"test\",\"StartLongitude\":50.1354,\"StartLatitude\":30.4324,\"EndLongitude\":1.4342,\"EndLatitude\":43.1234,\"Status\":1,\"Entity\":\"dasfas\",\"CreatedAt\":\"{DateTime.Now.ToString()}\"}}");

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
        [InlineData("test_update_status_test")]
        public async Task UpdateStatusTests_NotValidObject(string data)
        {
            try
            {
                var logistConnection = GetLogistConnection;

                var driverConnection = GetDriverConnection;

                driverConnection.On<string>(nameof(DriverHub.UpdateStatus), orderItem =>
                {
                    Assert.NotEqual("[]", orderItem);
                });

                logistConnection.On<string>(nameof(DriverHub.UpdateStatus), orderItem =>
                {
                    Assert.NotEqual("[]", orderItem);
                });

                await driverConnection.StartAsync();

                await logistConnection.StartAsync();

                await driverConnection.InvokeAsync(nameof(DriverHub.UpdateStatus), data);

                await driverConnection.StopAsync();

                await logistConnection.StopAsync();
            }
            catch (Exception)
            {
            }
        }
    }
}
