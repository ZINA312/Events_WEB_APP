
using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Data;

namespace Events_WEB_APP.Persistence.Repositories.RoleRepository
{
    /// <summary>
    /// Репозиторий для управления ролями в базе данных.
    /// </summary>
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RoleRepository"/>.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных для доступа к данным.</param>
        public RoleRepository(EventsAppDbContext dbContext) : base(dbContext) { }
    }
}
