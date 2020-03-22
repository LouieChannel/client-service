using Ascalon.ClientService.Features.Tasks.Dtos;
using MediatR;

namespace Ascalon.ClientService.Features.Tasks.GetTask
{
    /// <summary>
    /// Запрос получения данных о задаче.
    /// </summary>
    public class GetTaskQuery : IRequest<Task>
    {
        /// <summary>
        /// Возвращает или задаёт идентификатор задачи.
        /// </summary>
        public int Id { get; set; }
    }
}
