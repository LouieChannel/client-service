using Ascalon.ClientService.DataBaseContext;
using Ascalon.Uow.Ef;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Ascalon.ClientService.Repositories
{
    public class RolesRepository : EfRepository<Role>
    {
        public RolesRepository(DbContext context) : base(context)
        {
        }

        public virtual Task<Role> GetRoleByIdAsync(int roleId) => Entities.Where(a => a.Id == roleId).FirstOrDefaultAsync();

        public virtual Task<bool> ExistAsync(int roleId) => Entities.AnyAsync(a => a.Id == roleId);
    }
}
