using Ascalon.ClientService.Features.Tasks.Dtos;
using Ascalon.MefiatR.Validator.Fluent;
using Microsoft.Extensions.DependencyInjection;
using Ascalon.Uow.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

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
