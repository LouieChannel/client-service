using Ascalon.Uow.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = Ascalon.ClientService.DataBaseContext.Task;
using TaskThread = System.Threading.Tasks.Task;

namespace Ascalon.ClientService.Repositories
{
    public class TasksRepository : EfRepository<Task>
    {
        public TasksRepository(DbContext context) : base(context)
        {
        }

        public virtual Task<List<Task>> GetTasksByDriverIdAsync(int driverId) => Entities
            .Where(i => i.DriverId == driverId)
            .ToListAsync();

        public virtual Task<Task> GetTask(int driverId, short statusId) => Entities
            .Where(i => i.DriverId == driverId && i.Status == statusId)
            .FirstOrDefaultAsync();

        public virtual Task<Task> GetTaskByIdAsync(int id) => Entities.Where(i => i.Id == id)
            .FirstOrDefaultAsync();

        public virtual Task<EntityEntry<Task>> UpdateTaskAsync(Task task) => TaskThread.Run(() => Entities
            .Update(task));

        public virtual Task<List<Task>> GetTasks() => Entities.ToListAsync();

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
