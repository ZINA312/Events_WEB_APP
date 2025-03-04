using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Data;

namespace Events_WEB_APP.Persistence.Repositories.EventRepository
{
    public class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(EventsAppDbContext dbContext) : base(dbContext) { }
    }
}
