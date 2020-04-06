using DatabaseContext = Ascalon.ClientService.DataBaseContext;
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
                Driver = task.Driver.ToCommandUser(),
                Logist = task.Logist.ToCommandUser(),
                EndLatitude = task.EndLatitude,
                EndLongitude = task.EndLongitude,
                Entity = task.Entity,
                Id = task.Id,
                StartLatitude = task.StartLatitude,
                StartLongitude = task.StartLongitude,
                Status = (StatusType)task.Status
            };
        }

        public static User ToCommandUser(this DatabaseContext.User user)
        {
            return new User()
            {
                Id = user.Id,
                FullName = user.FullName,
                DumperId = user.DumperId
            };
        }
    }
}
