
using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Data;

namespace Events_WEB_APP.Persistence.Repositories.RoleRepository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(EventsAppDbContext dbContext) : base(dbContext) { }
    }
}
