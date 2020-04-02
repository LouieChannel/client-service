using Ascalon.MefiatR.Validator.Fluent;
using Microsoft.Extensions.DependencyInjection;

namespace Ascalon.ClientService.Features.Tasks.GetAllTask
{
    public static class Extensions
    {
        public static void ConfigureGetAllTask(this IServiceCollection services)
        {
            services.AddFluentValidatingPreProcessor<GetAllTaskQuery, GetAllTaskValidator>();
        }
    }
}
