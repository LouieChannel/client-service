using UsersDtos = Ascalon.ClientService.Features.Users.Dtos;

namespace Ascalon.ClientService.Features.Users.CreateUser.Dtos
{
    public static class MappingExtensions
    {
        public static UsersDtos.User ToCommandUser(this DataBaseContext.User user, string roleName)
        {
            return new UsersDtos.User()
            {
                Id = user.Id,
                FullName = user.FullName,
                Role = roleName
            };
        }

        public static DataBaseContext.User ToDataBaseContext(this CreateUserCommand user, int? dumperId)
        {
            return new DataBaseContext.User()
            {
                Login = user.Login,
                Password = user.Password,
                RoleId = (int)user.Role,
                FullName = user.FullName,
                DumperId = dumperId
            };
        }
    }
}
