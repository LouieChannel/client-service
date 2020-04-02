using Ascalon.ClientService.Repositories;
using Ascalon.MefiatR.Validator.Fluent.Exceptions;
using Ascalon.Uow;
using MediatR.Pipeline;
using System.Threading;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Features.Tasks.UpdateTask
{
    class UpdateTaskPreProcessor : IRequestPreProcessor<UpdateTaskCommand>
    {
        private readonly IUnitOfWork _uow;

        public UpdateTaskPreProcessor(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task Process(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            if (!await _uow.GetRepository<TasksRepository>().ExistAsync(request.Id))
                throw new BadRequestException(nameof(request.Id), "Задачи с таким идентификатором не существует.");
        }
    }
}

