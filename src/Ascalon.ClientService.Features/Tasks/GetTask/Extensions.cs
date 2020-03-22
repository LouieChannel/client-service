using Ascalon.MefiatR.Validator.Fluent;
using Microsoft.Extensions.DependencyInjection;

namespace Ascalon.ClientService.Features.Tasks.GetTask
{
    public static class Extensions
    {
        public static void ConfigureGetTask(this IServiceCollection services)
        {
            services.AddFluentValidatingPreProcessor<GetTaskQuery, GetTaskValidator>();
        }
    }
}
