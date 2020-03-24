using DataTask = Ascalon.ClientService.DataBaseContext.Task;

namespace Ascalon.ClientService.Features.Tasks.UpdateTask.Dtos
{
    public static class MappingExtensions
    {
        public static DataTask ToDataTask(this UpdateTaskCommand task)
        {
            return new DataTask()
            {
                CreatedAt = task.CreatedAt,
                Description = task.Description,
                DriverId = task.DriverId,
                EndLatitude = task.EndLatitude,
                EndLongitude = task.EndLongitude,
                Id = task.Id,
                StartLatitude = task.StartLatitude,
                StartLongitude = task.StartLongitude,
                Status = (short)task.Status,
                Entity = task.Entity
            };
        }
    }
}
