using Ascalon.ClientService.Repositories;
using Ascalon.MefiatR.Validator.Fluent.Exceptions;
using Ascalon.Uow;
using MediatR.Pipeline;
using System.Threading;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Features.Users.GetUser
{
    public class GetUserValidationPreProcessor : IRequestPreProcessor<GetUserQuery>
    {
        private readonly IUnitOfWork _uow;

        public GetUserValidationPreProcessor(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Process(GetUserQuery request, CancellationToken cancellationToken)
        {
            if (!await _uow.GetRepository<UsersRepository>().ExistAsync(request.Login, request.Password))
                throw new BadRequestException(nameof(request.Login), "Аккаунта с данным логином либо паролем, не существует.");
        }
    }
}
