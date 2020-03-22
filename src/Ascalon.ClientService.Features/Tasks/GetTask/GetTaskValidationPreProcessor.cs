using Ascalon.ClientService.Repositories;
using Ascalon.MefiatR.Validator.Fluent.Exceptions;
using Ascalon.Uow;
using MediatR.Pipeline;
using System.Threading;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Features.Tasks.GetTask
{
    public class GetTaskValidationPreProcessor : IRequestPreProcessor<GetTaskQuery>
    {
        private readonly IUnitOfWork _uow;

        public GetTaskValidationPreProcessor(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Process(GetTaskQuery request, CancellationToken cancellationToken)
        {
            if (!await _uow.GetRepository<TasksRepository>().ExistAsync(request.Id))
                throw new BadRequestException(nameof(request.Id), "Задачи с таким идентификатором не существует.");
        }
    }
}
