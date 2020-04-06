using Ascalon.ClientSerice;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Threading;
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
                    options.Cookies = new CookieContainer();
                    options.Cookies.Add(new Cookie("Id", "2") { Domain = "localhost" });
                    options.Cookies.Add(new Cookie("Role", "Driver") { Domain = "localhost" });
                    options.Cookies.Add(new Cookie("Name", "Test1") { Domain = "localhost" });
                })
                .Build();

            GetDriverConnectionWithLogistCookie = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/driver", options =>
                {
                    options.Cookies = new CookieContainer();
                    options.Cookies.Add(new Cookie("Id", "1") { Domain = "localhost" });
                    options.Cookies.Add(new Cookie("Role", "Logist") { Domain = "localhost" });
                    options.Cookies.Add(new Cookie("Name", "Test") { Domain = "localhost" });
                })
                .Build();

            GetLogistConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/logist", options =>
                {
                    options.Cookies = new CookieContainer();
                    options.Cookies.Add(new Cookie("Id", "1") { Domain = "localhost" });
                    options.Cookies.Add(new Cookie("Role", "Logist") { Domain = "localhost" });
                    options.Cookies.Add(new Cookie("Name", "Test") { Domain = "localhost" });
                })
                .Build();

            GetLogistConnectionWithDriverCookie = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/logist", options =>
                {
                    options.Cookies = new CookieContainer();
                    options.Cookies.Add(new Cookie("Id", "2") { Domain = "localhost" });
                    options.Cookies.Add(new Cookie("Role", "Driver") { Domain = "localhost" });
                    options.Cookies.Add(new Cookie("Name", "Test1") { Domain = "localhost" });
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
