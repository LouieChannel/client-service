using Ascalon.ClientService.DataBaseContext;
using Ascalon.ClientService.Features.Tasks.CreateTask;
using Ascalon.ClientService.Features.Tasks.GetTask;
using Ascalon.ClientService.Features.Tasks.UpdateTask;
using Ascalon.ClientService.Features.Users.GetUser;
using Ascalon.ClientService.Hubs;
using Ascalon.Uow;
using Ascalon.Uow.Ef;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ascalon.ClientSerice
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddLogging();
            services.AddMemoryCache();

            services.AddDbContext<ClientServiceContext>(options => options.UseNpgsql(Configuration.GetConnectionString("ClientService")));
            services.AddScoped<IUnitOfWork>(s => new EfUnitOfWork<ClientServiceContext>(s.GetRequiredService<ClientServiceContext>()));

            services.AddMediatR(Assembly.GetAssembly(typeof(GetUserHandler)));

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder
                        .WithOrigins("http://localhost:5000")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddSignalR();

            services.ConfigureGetUser();
            services.ConfigureGetTask();
            services.ConfigureCreateTask();
            services.ConfigureUpdateTask();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<LogistHub>("/logist");
                endpoints.MapGet("/", context => context.Response.WriteAsync("Welcome in Client Service!"));
                endpoints.MapControllers();
            }); 
        }
    }
}
