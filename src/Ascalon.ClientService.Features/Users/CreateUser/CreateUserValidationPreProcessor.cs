using Ascalon.ClientService.Repositories;
using Ascalon.MefiatR.Validator.Fluent.Exceptions;
using Ascalon.Uow;
using MediatR.Pipeline;
using System.Threading;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Features.Users.CreateUser
{
    public class CreateUserValidationPreProcessor : IRequestPreProcessor<CreateUserCommand>
    {
        private readonly IUnitOfWork _uow;

        public CreateUserValidationPreProcessor(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Process(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _uow.GetRepository<UsersRepository>().ExistAsync(request.Login, request.Password))
                throw new BadRequestException(nameof(request.Login), "Аккаунт с данным логином либо паролем, уже существует.");
        }
    }
}
