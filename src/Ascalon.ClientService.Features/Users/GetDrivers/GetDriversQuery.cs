using Ascalon.ClientService.Features.Users.Dtos;
using MediatR;
using System.Collections.Generic;

namespace Ascalon.ClientService.Features.Users.GetDrivers
{
    public class GetDriversQuery : IRequest<List<User>>
    {
    }
}
