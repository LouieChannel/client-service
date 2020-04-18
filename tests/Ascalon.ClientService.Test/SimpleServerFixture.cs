using Ascalon.ClientSerice;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
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

            AuthData logist = null;

            AuthData driver = null;

            if (_webHost == null)
            {
                _webHost = CreateHostBuilder().Build();
                _webHost.Start();

                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:5000")
                };

                logist = GetDataByResponse<AuthData>(httpClient.PostAsync("/auth", new StringContent("{ \"login\":\"a.mirko@dostaevsky.ru\", \"password\":\"123456\" }",
                  System.Text.Encoding.UTF8, "application/json")).Result, nameof(SimpleServerFixture));

                driver = GetDataByResponse<AuthData>(httpClient.PostAsync("/auth", new StringContent("{ \"login\":\"test@gmail.com\", \"password\":\"654321\" }",
                       System.Text.Encoding.UTF8, "application/json")).Result, nameof(SimpleServerFixture));
            }

            mutexObj.ReleaseMutex();

            GetDriverConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/driver/", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(driver.Access_token);
                })
                .Build();

            GetDriverConnectionWithLogistCookie = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/driver/", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(logist.Access_token);
                })
                .Build();

            GetLogistConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/logist/", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(logist.Access_token);
                })
                .Build();

            GetLogistConnectionWithDriverCookie = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/logist/", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(driver.Access_token);
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

        private T GetDataByResponse<T>(HttpResponseMessage httpResponseMessage, string methodName)
        {
            if (!httpResponseMessage.IsSuccessStatusCode)
                throw new Exception($"{methodName} got not successful status code: StatusCode = {httpResponseMessage.StatusCode}, RequestMessage = {JsonConvert.SerializeObject(httpResponseMessage.RequestMessage)}.");

            var responseObject = httpResponseMessage.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(responseObject))
                throw new NullReferenceException($"{methodName} ordersapi returned empty or null content.");

            return JsonConvert.DeserializeObject<T>(responseObject);
        }
    }
}
