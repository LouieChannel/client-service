using Ascalon.ClientService.Features.Exceptions;
using Ascalon.ClientService.Features.Users.CreateUser.Dtos;
using Ascalon.ClientService.Features.Users.Dtos;
using Ascalon.ClientService.Repositories;
using Ascalon.Uow;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Features.Users.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, User>
    {
        private readonly RolesRepository _rolesRepository;
        private readonly UsersRepository _userRepository;
        private IMemoryCache _memoryCache;

        public CreateUserHandler(IUnitOfWork uow, IMemoryCache memoryCache)
        {
            _userRepository = uow.GetRepository<UsersRepository>();
            _rolesRepository = uow.GetRepository<RolesRepository>();

            _memoryCache = memoryCache;
        }

        public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var dumperId = default(int?);

            if (request.Role == RoleType.Driver)
                dumperId = await _memoryCache.GetOrCreateAsync("dumperId", options =>
                {
                    return _userRepository.GetMaxDumperId();
                });

            var user = await _userRepository.CreateUserAsync(request.ToDataBaseContext(dumperId));

            if (request.Role == RoleType.Driver)
                _memoryCache.Set("dumperId", dumperId++);

            if (user == null)
                throw new NotFoundException();

            var role = await _rolesRepository.GetRoleByIdAsync(user.Entity.RoleId);

            if (role == null)
                throw new NotFoundException();

            return user.Entity.ToCommandUser(role.Name);
        }
    }
}
