using Ascalon.ClientService.Features.Tasks.Dtos;
using System;
using DatabaseContext = Ascalon.ClientService.DataBaseContext;

namespace Ascalon.ClientService.Features.Tasks.CreateTask.Dtos
{
    public static class MappingExtensions
    {
        public static DatabaseContext.Task ToEntityTask(this CreateTaskCommand task)
        {
            return new DatabaseContext.Task() 
            { 
                CreatedAt = DateTime.Now,
                Description = task.Description,
                DriverId = task.DriverId,
                EndLatitude = task.EndLatitude,
                EndLongitude = task.EndLongitude,
                StartLatitude = task.StartLatitude,
                StartLongitude = task.StartLongitude,
                Status = (short)task.Status,
                Entity = task.Entity
            };
        }

        public static Task ToCommandTask(this DatabaseContext.Task task)
        {
            return new Task()
            {
                Id = task.Id,
                Entity = task.Entity,
                CreatedAt = task.CreatedAt,
                Description = task.Description,
                DriverId = task.DriverId,
                EndLatitude = task.EndLatitude,
                EndLongitude = task.EndLongitude,
                StartLatitude = task.StartLatitude,
                StartLongitude = task.StartLongitude,
                Status = (StatusType)task.Status
            };
        }
    }
}
