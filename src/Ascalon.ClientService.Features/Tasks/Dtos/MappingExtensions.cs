using DataTask = Ascalon.ClientService.DataBaseContext.Task;

namespace Ascalon.ClientService.Features.Tasks.Dtos
{
    public static class MappingExtensions
    {
        public static Task ToQueryTask(this DataTask task)
        {
            return new Task()
            {
                CreatedAt = task.CreatedAt,
                Description = task.Description,
                DriverId = task.DriverId,
                EndLatitude = task.EndLatitude,
                EndLongitude = task.EndLongitude,
                Entity = task.Entity,
                Id = task.Id,
                StartLatitude = task.StartLatitude,
                StartLongitude = task.StartLongitude,
                Status = (StatusType)task.Status
            };
        }
    }
}
