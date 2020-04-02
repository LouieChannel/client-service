using Ascalon.Uow.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = Ascalon.ClientService.DataBaseContext.Task;
using TaskThread = System.Threading.Tasks.Task;

namespace Ascalon.ClientService.Repositories
{
    public class TasksRepository : EfRepository<Task>
    {
        private const short StatusTypeDone = 2;

        public TasksRepository(DbContext context) : base(context)
        {
        }

        public virtual Task<List<Task>> GetTasksByDriverIdAsync(int driverId) => Entities
            .Where(i => i.DriverId == driverId && i.Status != StatusTypeDone)
            .ToListAsync();

        public virtual Task<Task> GetTaskAsync(int driverId, short statusId, DateTime dateTime) =>
            Entities.Where(i => i.DriverId == driverId &&
            i.Status == statusId && dateTime == i.CreatedAt)
            .FirstOrDefaultAsync();

        public virtual Task<Task> GetTaskByIdAsync(int id) => Entities.Where(i => i.Id == id)
            .FirstOrDefaultAsync();

        public virtual Task<EntityEntry<Task>> UpdateTaskAsync(Task task) =>
            TaskThread.Run(() => Entities
            .Update(task));

        public virtual async Task<List<Task>> GetTasks(DateTime dateTimeFiltered) => (await Entities.ToListAsync())
            .Where(i => (i.CreatedAt - dateTimeFiltered).TotalDays == 0)
            .ToList();

        public virtual Task<Task> CreateTaskAsync(Task task)
        {
            return TaskThread.Run(() =>
            {
                Add(task);
                return task;
            });
        }

        public virtual Task<bool> ExistAsync(int id) => Entities.AnyAsync(a => a.Id == id);
    }
}
