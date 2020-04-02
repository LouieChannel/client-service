using Ascalon.MefiatR.Validator.Fluent;
using Microsoft.Extensions.DependencyInjection;

namespace Ascalon.ClientService.Features.Tasks.GetDriverTask
{
    public static class Extensions
    {
        public static void ConfigureGetDriverTask(this IServiceCollection services)
        {
            services.AddFluentValidatingPreProcessor<GetDriverTaskQuery, GetDriverTaskValidator>();
        }
    }
}
