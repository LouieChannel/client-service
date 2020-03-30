using Ascalon.ClientService.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Ascalon.ClientService.Test
{
    public class HubTests : SimpleServerFixture
    {
        public HubTests(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task CreateTask()
        {
            try
            {
                var driverConnection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/logist", options =>
                    {
                        options.Cookies = new CookieContainer();
                        options.Cookies.Add(new Cookie("Id", "2") { Domain = "localhost" });
                        options.Cookies.Add(new Cookie("Role", "Driver") { Domain = "localhost" });
                        options.Cookies.Add(new Cookie("Name", "Test1") { Domain = "localhost" });
                    })
                    .Build();

                var logistConnection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/logist", options => 
                    {
                        options.Cookies = new CookieContainer();
                        options.Cookies.Add(new Cookie("Id", "1") { Domain = "localhost" });
                        options.Cookies.Add(new Cookie("Role", "Logist") { Domain = "localhost" });
                        options.Cookies.Add(new Cookie("Name", "Test") { Domain = "localhost" });
                    })
                    .Build();

                var logist2Connection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/logist", options =>
                    {
                        options.Cookies = new CookieContainer();
                        options.Cookies.Add(new Cookie("Id", "3") { Domain = "localhost" });
                        options.Cookies.Add(new Cookie("Role", "Logist") { Domain = "localhost" });
                        options.Cookies.Add(new Cookie("Name", "Test") { Domain = "localhost" });
                    })
                    .Build();
                
                driverConnection.On<object>(nameof(LogistHub.CreateTask), orderItem =>
                {
                    Assert.NotNull(orderItem);
                });

                logistConnection.On<object>(nameof(LogistHub.CreateTask), orderItem =>
                {
                    Assert.NotNull(orderItem);
                });

                logist2Connection.On<object>(nameof(LogistHub.CreateTask), orderItem =>
                {
                    Assert.NotNull(orderItem);
                });

                await driverConnection.StartAsync();

                await logistConnection.StartAsync();

                await logist2Connection.StartAsync();

                await driverConnection.InvokeAsync(nameof(LogistHub.CreateTask), "{\"DriverId\":2,\"Description\":\"test21\",\"StartLongitude\":50.1354,\"StartLatitude\":30.4324,\"EndLongitude\":1.4342,\"EndLatitude\":43.1234,\"Status\":1,\"Entity\":\"dasfas\"}");

                await logistConnection.InvokeAsync(nameof(LogistHub.CreateTask), "{\"DriverId\":2,\"Description\":\"test\",\"StartLongitude\":50.1354,\"StartLatitude\":30.4324,\"EndLongitude\":1.4342,\"EndLatitude\":43.1234,\"Status\":2,\"Entity\":\"dasfas\"}");

                Thread.Sleep(10000);

                await logist2Connection.InvokeAsync(nameof(LogistHub.CreateTask), "{\"DriverId\":2,\"Description\":\"test\",\"StartLongitude\":50.1354,\"StartLatitude\":30.4324,\"EndLongitude\":1.4342,\"EndLatitude\":43.1234,\"Status\":2,\"Entity\":\"dasfas\"}");

                await driverConnection.StopAsync();

                await logistConnection.StopAsync();

                await logist2Connection.StopAsync();
            }
            catch(Exception ex)
            {

            }
        }
    }
}
