using Ascalon.MefiatR.Validator.Fluent;
using Microsoft.Extensions.DependencyInjection;

namespace Ascalon.ClientService.Features.Users.CreateUser
{
    public static class Extensions
    {
        public static void ConfigureCreateUser(this IServiceCollection services)
        {
            services.AddFluentValidatingPreProcessor<CreateUserCommand, CreateUserValidator>();
        }
    }
}
