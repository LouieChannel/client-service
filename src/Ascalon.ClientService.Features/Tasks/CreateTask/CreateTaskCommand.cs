using Ascalon.ClientService.Features.Tasks.Dtos;
using MediatR;

namespace Ascalon.ClientService.Features.Tasks.CreateTask
{
    public class CreateTaskCommand : IRequest<Task>
    {
        /// <summary>
        /// Возвращает или задаёт идентификатор водителя.
        /// </summary>
        public int DriverId { get; set; }

        /// <summary>
        /// Возвращает или задаёт описание задания.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Возвращает или задаёт начальную долготу.
        /// </summary>
        public float StartLongitude { get; set; }

        /// <summary>
        /// Возвращает или задаёт начальную широту.
        /// </summary>
        public float StartLatitude { get; set; }

        /// <summary>
        /// Возвращает или задаёт конечную долготу.
        /// </summary>
        public float EndLongitude { get; set; }

        /// <summary>
        /// Возвращает или задаёт конеченую широту.
        /// </summary>
        public float EndLatitude { get; set; }

        /// <summary>
        /// Возвращает или задаёт статус задачи.
        /// </summary>
        public StatusType Status { get; set; }

        /// <summary>
        /// Возвращает или задаёт основное задание.
        /// </summary>
        public string Entity { get; set; }
    }
}
