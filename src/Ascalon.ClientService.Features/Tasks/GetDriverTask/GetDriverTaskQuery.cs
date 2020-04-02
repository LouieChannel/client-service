using Ascalon.ClientService.Features.Tasks.Dtos;
using MediatR;
using System.Collections.Generic;

namespace Ascalon.ClientService.Features.Tasks.GetDriverTask
{
    /// <summary>
    /// Запрос получения задач для конкретного водителя.
    /// </summary>
    public class GetDriverTaskQuery : IRequest<List<Task>>
    {
        /// <summary>
        /// Возвращает или задаёт идентификатор водителя.
        /// </summary>
        public int DriverId { get; set; }
    }
}
