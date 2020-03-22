﻿using Ascalon.ClientService.DataBaseContext;
using Ascalon.Uow.Ef;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Repositories
{
    public class UsersRepository : EfRepository<User>
    {
        public UsersRepository(DbContext context) : base(context)
        {
        }

        public virtual Task<User> GetUserAsync(string login, string password) => Entities
            .Where(a => a.Login == login && a.Password == password)
            .FirstOrDefaultAsync();

        public virtual Task<User> GetDriverWithoutDumper(int roleId) => Entities
            .Where(i => !i.DumperId.HasValue && i.RoleId == roleId)
            .FirstOrDefaultAsync();

        public virtual Task<bool> ExistAsync(string login, string password) => Entities
            .AnyAsync(a => a.Login == login && a.Password == password);
    }
}
