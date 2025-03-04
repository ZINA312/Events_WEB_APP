using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Data;

namespace Events_WEB_APP.Persistence.Repositories.ParticipantRepository
{
    public class ParticipantRepository : Repository<Participant>, IParticipantRepository
    {
        public ParticipantRepository(EventsAppDbContext dbContext) : base(dbContext) { }
    }
}
