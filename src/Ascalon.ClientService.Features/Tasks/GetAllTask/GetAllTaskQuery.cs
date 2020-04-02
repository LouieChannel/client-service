using Ascalon.ClientService.Features.Tasks.Dtos;
using MediatR;
using System;
using System.Collections.Generic;

namespace Ascalon.ClientService.Features.Tasks.GetAllTask
{
    /// <summary>
    /// Запрос получения всех задач.
    /// </summary>
    public class GetAllTaskQuery : IRequest<List<Task>>
    {
        /// <summary>
        /// Возвращает или задаёт фильтр по дате.
        /// </summary>
        public DateTime DateFilter { get; set; }
    }
}
