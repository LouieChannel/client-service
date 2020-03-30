using Ascalon.ClientSerice;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using Xunit.Abstractions;

namespace Ascalon.ClientService.Test
{
    public class SimpleServerFixture : IDisposable
    {
        private IHost _webHost;
        private IConfiguration _configuration;
        private ITestOutputHelper _output;

        public SimpleServerFixture(ITestOutputHelper outputHelper)
        {
            _output = outputHelper;
            _webHost = CreateHostBuilder().Build();
            _webHost.Start();
        }

        public IHostBuilder CreateHostBuilder(params string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webHostBuilder => webHostBuilder.UseStartup<Startup>());

        public void Dispose()
        {
            _webHost?.StopAsync().Wait();
            _webHost?.Dispose();
        }

        public T GetService<T>() => (T)_webHost!.Services.GetService(typeof(T));
    }
}
