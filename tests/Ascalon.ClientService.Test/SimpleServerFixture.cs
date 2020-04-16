using Ascalon.ClientSerice;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Ascalon.ClientService.Test
{
    public class SimpleServerFixture : IDisposable
    {
        static Mutex mutexObj = new Mutex();
        private static IHost _webHost;
        private IConfiguration _configuration;
        private ITestOutputHelper _output;

        public SimpleServerFixture(ITestOutputHelper outputHelper)
        {
            _output = outputHelper;

            mutexObj.WaitOne();

            if (_webHost == null)
            {
                _webHost = CreateHostBuilder().Build();
                _webHost.Start();
            }

            mutexObj.ReleaseMutex();

            GetDriverConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/driver", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(@"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoi0JzQuNGA0LrQviDQkC7QkC4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJMb2dpc3QiLCJuYmYiOjE1ODY5ODE3NDgsImV4cCI6MTU5MDU4MTc0OCwiaXNzIjoiQXNjYWxvbi5DbGllbnRTZXJ2aWNlIiwiYXVkIjoiQXNjYWxvbi5DbGllbnRXZWIifQ.-E1c998XRLIGx3wprvbNgpqWtUqfCMAfu_ngWHx-YJw");
                })
                .Build();

            GetDriverConnectionWithLogistCookie = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/driver", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(@"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoi0JzQuNGA0LrQviDQkC7QkC4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJMb2dpc3QiLCJuYmYiOjE1ODY5ODE3NDgsImV4cCI6MTU5MDU4MTc0OCwiaXNzIjoiQXNjYWxvbi5DbGllbnRTZXJ2aWNlIiwiYXVkIjoiQXNjYWxvbi5DbGllbnRXZWIifQ.-E1c998XRLIGx3wprvbNgpqWtUqfCMAfu_ngWHx-YJw");
                })
                .Build();

            GetLogistConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/logist", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(@"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoi0JzQuNGA0LrQviDQkC7QkC4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJMb2dpc3QiLCJuYmYiOjE1ODY5ODE3NDgsImV4cCI6MTU5MDU4MTc0OCwiaXNzIjoiQXNjYWxvbi5DbGllbnRTZXJ2aWNlIiwiYXVkIjoiQXNjYWxvbi5DbGllbnRXZWIifQ.-E1c998XRLIGx3wprvbNgpqWtUqfCMAfu_ngWHx-YJw");
                })
                .Build();

            GetLogistConnectionWithDriverCookie = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/logist", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(@"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoi0JzQuNGA0LrQviDQkC7QkC4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJMb2dpc3QiLCJuYmYiOjE1ODY5ODE3NDgsImV4cCI6MTU5MDU4MTc0OCwiaXNzIjoiQXNjYWxvbi5DbGllbnRTZXJ2aWNlIiwiYXVkIjoiQXNjYWxvbi5DbGllbnRXZWIifQ.-E1c998XRLIGx3wprvbNgpqWtUqfCMAfu_ngWHx-YJw");
                })
                .Build();
        }

        public IHostBuilder CreateHostBuilder(params string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder.UseStartup<Startup>());

        public void Dispose()
        {
            _webHost?.StopAsync().Wait();
            _webHost?.Dispose();
        }

        public T GetService<T>() => (T)_webHost!.Services.GetService(typeof(T));

        public HubConnection GetDriverConnection { get; private set; }

        public HubConnection GetDriverConnectionWithLogistCookie { get; private set; }

        public HubConnection GetLogistConnection { get; private set; }

        public HubConnection GetLogistConnectionWithDriverCookie { get; private set; }
    }
}
