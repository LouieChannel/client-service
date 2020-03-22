using Ascalon.ClientService.Features.Users.Dtos;
using MediatR;

namespace Ascalon.ClientService.Features.Users.GetUser
{
    public class GetUserQuery : IRequest<User>
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
