
using Events_WEB_APP.Core.Entities;

namespace Events_WEB_APP.Persistence.Repositories.UserRepository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
