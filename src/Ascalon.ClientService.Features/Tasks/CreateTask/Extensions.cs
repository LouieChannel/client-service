using Ascalon.ClientService.Features.Tasks.Dtos;
using Ascalon.MefiatR.Validator.Fluent;
using Ascalon.Uow.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Ascalon.ClientService.Features.Tasks.CreateTask
{
    public static class Extensions
    {
        public static void ConfigureCreateTask(this IServiceCollection services)
        {
            services.AddFluentValidatingPreProcessor<CreateTaskCommand, CreateTaskValidator>();
            services.AddUnitOfWorkCommitter<CreateTaskCommand, Task>();
        }
    }
}
