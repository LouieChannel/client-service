using Ascalon.ClientService.Features.Users.Dtos;
using UsersDtos = Ascalon.ClientService.Features.Users.Dtos;

namespace Ascalon.ClientService.Features.Users.GetDrivers.Dtos
{
    public static class MappingExtensions
    {
        public static UsersDtos.User ToQueryUser(this DataBaseContext.User user)
        {
            return new UsersDtos.User()
            {
                Id = user.Id,
                FullName = user.FullName,
                Role = RoleType.Driver.ToString()
            };
        }
    }
}
