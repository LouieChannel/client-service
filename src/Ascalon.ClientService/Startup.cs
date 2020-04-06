using Ascalon.ClientService.DataBaseContext;
using Ascalon.ClientService.Features.Tasks.CreateTask;
using Ascalon.ClientService.Features.Tasks.GetAllTask;
using Ascalon.ClientService.Features.Tasks.GetDriverTask;
using Ascalon.ClientService.Features.Tasks.GetTask;
using Ascalon.ClientService.Features.Tasks.UpdateTask;
using Ascalon.ClientService.Features.Users.GetUser;
using Ascalon.ClientService.Hubs;
using Ascalon.ClientService.Middlewares;
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
                        .WithOrigins(Configuration.GetSection("ClientWebsite:Host").Value)
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddSignalR();

            services.ConfigureGetUser();
            services.ConfigureGetTask();
            services.ConfigureGetAllTask();
            services.ConfigureCreateTask();
            services.ConfigureUpdateTask();
            services.ConfigureGetDriverTask();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<BadRequestMiddleware>();
            app.UseMiddleware<NotFoundMiddleware>();
            app.UseMiddleware<ForbiddenMiddleware>();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<LogistHub>("/logist");
                endpoints.MapHub<DriverHub>("/driver");
                endpoints.MapGet("/", context => context.Response.WriteAsync("Welcome in Client Service!"));
                endpoints.MapControllers();
            });
        }
    }
}
