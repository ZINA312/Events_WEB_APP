using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Data;

namespace Events_WEB_APP.Persistence.Repositories.EventRepository
{
    /// <summary>
    /// Репозиторий для управления событиями в базе данных.
    /// </summary>
    public class EventRepository : Repository<Event>, IEventRepository
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EventRepository"/>.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных для доступа к данным.</param>
        public EventRepository(EventsAppDbContext dbContext) : base(dbContext) { }
    }
}
