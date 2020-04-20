using Ascalon.ClientService.Features.Exceptions;
using Ascalon.ClientService.Features.Users.Dtos;
using Ascalon.ClientService.Features.Users.GetDrivers.Dtos;
using Ascalon.ClientService.Repositories;
using Ascalon.Uow;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Features.Users.GetDrivers
{
    public class GetDriversHandler : IRequestHandler<GetDriversQuery, List<User>>
    {
        private readonly UsersRepository _userRepository;
        private readonly RolesRepository _roleRepository;
        private readonly IMemoryCache _memoryCache;

        public GetDriversHandler(IUnitOfWork uow, IMemoryCache memoryCache)
        {
            _userRepository = uow.GetRepository<UsersRepository>();
            _roleRepository = uow.GetRepository<RolesRepository>();
            _memoryCache = memoryCache;
        }

        public async Task<List<User>> Handle(GetDriversQuery request, CancellationToken cancellationToken)
        {
            var countEntities = await _userRepository.GetCountEntities();

            return await _memoryCache.GetOrCreate(countEntities.ToString(), async options =>
            {
                var users = await _userRepository.GetDriversAsync();

                if (users == null)
                    throw new NotFoundException();

                return users.Select(i => i.ToQueryUser()).ToList();
            });
        }
    }
}
