using Ascalon.ClientService.DataBaseContext;
using Ascalon.Uow.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Repositories
{
    public class UsersRepository : EfRepository<User>
    {
        private const int DriverRoleId = 2;

        private readonly DbContext _dbContext;

        public UsersRepository(DbContext context) : base(context)
        {
            _dbContext = context;
        }

        public virtual Task<User> GetUserAsync(string login, string password) => Entities
            .Where(a => a.Login == login && a.Password == password)
            .FirstOrDefaultAsync();

        public virtual Task<User> GetUserByIdAsync(int id) => Entities.Where(i => i.Id == id)
            .FirstOrDefaultAsync();

        public virtual Task<User> GetDriverWithoutDumperAsync(int roleId) => Entities
            .Where(i => !i.DumperId.HasValue && i.RoleId == roleId)
            .FirstOrDefaultAsync();

        public virtual Task<int> GetMaxDumperId() => Entities.Where(i => i.DumperId.HasValue && i.RoleId == DriverRoleId)
            .MaxAsync(i => i.DumperId.Value);

        public virtual Task<List<User>> GetDriversAsync() => Entities.Where(i => i.RoleId == DriverRoleId)
            .ToListAsync();

        public virtual ValueTask<EntityEntry<User>> CreateUserAsync(User user)
        {
            var result = _dbContext.AddAsync(user);

            _dbContext.SaveChanges();

            return result;
        }

        public virtual Task<bool> ExistAsync(string login, string password) => Entities
            .AnyAsync(a => a.Login == login && a.Password == password);
    }
}
