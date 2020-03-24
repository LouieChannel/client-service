using System;
using DatabaseContext = Ascalon.ClientService.DataBaseContext;
using DtosTasks = Ascalon.ClientService.Features.Tasks.Dtos;

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
                Status = task.Status,
                Entity = task.Entity
            };
        }

        public static DtosTasks.Task ToCommandTask(this DatabaseContext.Task task)
        {
            return new DtosTasks.Task()
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
                Status = task.Status
            };
        }
    }
}
