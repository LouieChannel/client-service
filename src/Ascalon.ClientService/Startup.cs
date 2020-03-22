using Ascalon.ClientService.DataBaseContext;
using Ascalon.ClientService.Features.Tasks.GetTask;
using Ascalon.ClientService.Features.Users.GetUser;
using Ascalon.Uow;
using Ascalon.Uow.Ef;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ascalon.ClientSerice
{
    public class Startup
    {
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

            services.ConfigureGetUser();
            services.ConfigureGetTask();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
