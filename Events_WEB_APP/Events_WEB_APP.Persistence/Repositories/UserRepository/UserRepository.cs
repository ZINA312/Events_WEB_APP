using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Events_WEB_APP.Persistence.Repositories.UserRepository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(EventsAppDbContext dbContext) : base(dbContext) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _entities.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
