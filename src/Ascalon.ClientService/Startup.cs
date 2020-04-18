using Ascalon.ClientService.DataBaseContext;
using Ascalon.ClientService.Features.Tasks.CreateTask;
using Ascalon.ClientService.Features.Tasks.GetAllTask;
using Ascalon.ClientService.Features.Tasks.GetDriverTask;
using Ascalon.ClientService.Features.Tasks.GetTask;
using Ascalon.ClientService.Features.Tasks.UpdateTask;
using Ascalon.ClientService.Features.Users.GetUser;
using Ascalon.ClientService.Hubs;
using Ascalon.ClientService.Infrastructure;
using Ascalon.ClientService.Kafka;
using Ascalon.ClientService.Kafka.Services;
using Ascalon.ClientService.Middlewares;
using Ascalon.Uow;
using Ascalon.Uow.Ef;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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

            services.Configure<ClientWebsite>(Configuration.GetSection(nameof(ClientWebsite)));

            services.AddDbContext<ClientServiceContext>(options => options.UseNpgsql(Configuration.GetConnectionString("ClientService")));
            services.AddScoped<IUnitOfWork>(s => new EfUnitOfWork<ClientServiceContext>(s.GetRequiredService<ClientServiceContext>()));

            services.AddMediatR(Assembly.GetAssembly(typeof(GetUserHandler)));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    });

            var clientHost = Configuration.GetSection("ClientWebsite").Get<ClientWebsite>();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(clientHost.Host)
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddSignalR();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Logist", policy =>
                      policy.RequireClaim("Logist"));

                options.AddPolicy("Driver", policy =>
                      policy.RequireClaim("Driver"));
            });

            services.ConfigureGetUser();
            services.ConfigureGetTask();
            services.ConfigureGetAllTask();
            services.ConfigureCreateTask();
            services.ConfigureUpdateTask();
            services.ConfigureGetDriverTask();

            services.Configure<DumperStateConsumerServiceOptions>(
               Configuration.GetSection(nameof(DumperStateConsumerServiceOptions)));


            services.AddHostedService<DumperStateConsumerService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<BadRequestMiddleware>();
            app.UseMiddleware<NotFoundMiddleware>();
            app.UseMiddleware<ForbiddenMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

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
