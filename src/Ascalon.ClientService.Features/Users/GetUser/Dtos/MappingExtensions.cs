using UsersDtos = Ascalon.ClientService.Features.Users.Dtos;

namespace Ascalon.ClientService.Features.Users.GetUser.Dtos
{
    public static class MappingExtensions
    {
        public static UsersDtos.User ToQueryUser(this DataBaseContext.User user, string roleName)
        {
            return new UsersDtos.User()
            {
                FullName = user.FullName,
                Role = roleName
            };
        }
    }
}
