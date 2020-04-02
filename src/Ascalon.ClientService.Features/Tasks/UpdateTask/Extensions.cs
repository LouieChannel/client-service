using Ascalon.MefiatR.Validator.Fluent;
using Microsoft.Extensions.DependencyInjection;

namespace Ascalon.ClientService.Features.Tasks.UpdateTask
{
    public static class Extensions
    {
        public static void ConfigureUpdateTask(this IServiceCollection services)
        {
            services.AddFluentValidatingPreProcessor<UpdateTaskCommand, UpdateTaskValidator>();
        }
    }
}
