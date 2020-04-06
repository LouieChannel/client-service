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

        private readonly DbContext _dbContext;


        public TasksRepository(DbContext context) : base(context)
        {
            _dbContext = context;
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

        public virtual async Task<List<Task>> GetTasks(DateTime filteredDate) => (await Entities.ToListAsync()
            ).Where(i => (filteredDate.Date <= i.CreatedAt) && (i.CreatedAt < filteredDate.AddDays(1).Date)).ToList();

        public virtual ValueTask<EntityEntry<Task>> CreateTaskAsync(Task task)
        {
            var result = _dbContext.AddAsync(task);

            _dbContext.SaveChanges();

            return result;
        }

        public virtual Task<bool> ExistAsync(int id) => Entities.AnyAsync(a => a.Id == id);
    }
}
