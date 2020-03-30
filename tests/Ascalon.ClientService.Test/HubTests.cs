using Ascalon.ClientService.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Net;
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
                var logistConnection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5000/logist", options => 
                    {
                        options.Cookies = new CookieContainer();
                        options.Cookies.Add(new Cookie("Id", "1") { Domain = "localhost" });
                        options.Cookies.Add(new Cookie("Role", "Logist") { Domain = "localhost" });
                        options.Cookies.Add(new Cookie("Name", "Test") { Domain = "localhost" });
                    })
                    .Build();

                logistConnection.On<object>(nameof(LogistHub.CreateTask), orderItem =>
                {
                    Assert.NotNull(orderItem);
                });

                await logistConnection.StartAsync();

                await logistConnection.InvokeAsync(nameof(LogistHub.CreateTask), "{\"DriverId\":1,\"Description\":\"test\",\"StartLongitude\":50.1354,\"StartLatitude\":30.4324,\"EndLongitude\":1.4342,\"EndLatitude\":43.1234,\"Status\":1,\"Entity\":\"dasfas\"}");

                await Task.Delay(3000);

                await logistConnection.StopAsync();
            }
            catch(Exception ex)
            {

            }
        }
    }
}
