using Ascalon.ClientService.Features.Users.Dtos;
using Ascalon.ClientService.Features.Users.GetUser.Dtos;
using Ascalon.ClientService.Repositories;
using Ascalon.Uow;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Features.Users.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, User>
    {
        private readonly UsersRepository _userRepository;
        private readonly RolesRepository _roleRepository;
        private readonly IMemoryCache _memoryCache;

        public GetUserHandler(IUnitOfWork uow, IMemoryCache memoryCache)
        {
            _userRepository = uow.GetRepository<UsersRepository>();
            _roleRepository = uow.GetRepository<RolesRepository>();
            _memoryCache = memoryCache;
        }

        public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            return await _memoryCache.GetOrCreate($"{request.Login}-{request.Password}", async options =>
            {
                var user = await _userRepository.GetUserAsync(request.Login, request.Password);

                if (user == null)
                    throw new System.Exception();

                var role = await _roleRepository.GetRoleByIdAsync(user.RoleId);

                if (role == null)
                    throw new System.Exception();

                return user.ToQueryUser(role.Name);
            });
        }
    }
}
