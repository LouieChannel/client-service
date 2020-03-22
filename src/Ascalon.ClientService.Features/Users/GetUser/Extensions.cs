using Ascalon.MefiatR.Validator.Fluent;
using Microsoft.Extensions.DependencyInjection;

namespace Ascalon.ClientService.Features.Users.GetUser
{
    public static class Extensions
    {
        public static void ConfigureGetUser(this IServiceCollection services)
        {
            services.AddFluentValidatingPreProcessor<GetUserQuery, GetUserValidator>();
        }
    }
}
