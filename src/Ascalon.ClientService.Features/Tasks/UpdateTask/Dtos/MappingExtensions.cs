using Ascalon.ClientService.Features.Tasks.Dtos;
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
                DriverId = task.Driver.Id,
                LogistId = task.Logist.Id,
                EndLatitude = task.EndLatitude,
                EndLongitude = task.EndLongitude,
                Id = task.Id,
                StartLatitude = task.StartLatitude,
                StartLongitude = task.StartLongitude,
                Status = (short)task.Status,
                Entity = task.Entity
            };
        }

        public static Task ToCommandTask(this DataTask task)
        {
            return new Task()
            {
                CreatedAt = task.CreatedAt,
                Description = task.Description,
                Driver = task.Driver.ToCommandUser(),
                Logist = task.Logist.ToCommandUser(),
                EndLatitude = task.EndLatitude,
                EndLongitude = task.EndLongitude,
                Id = task.Id,
                StartLatitude = task.StartLatitude,
                StartLongitude = task.StartLongitude,
                Status = (StatusType)task.Status,
                Entity = task.Entity
            };
        }
    }
}
