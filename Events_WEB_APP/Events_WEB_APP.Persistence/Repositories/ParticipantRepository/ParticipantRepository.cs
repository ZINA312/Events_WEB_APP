using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Data;

namespace Events_WEB_APP.Persistence.Repositories.ParticipantRepository
{
    /// <summary>
    /// Репозиторий для управления участниками в базе данных.
    /// </summary>
    public class ParticipantRepository : Repository<Participant>, IParticipantRepository
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParticipantRepository"/>.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных для доступа к данным.</param>
        public ParticipantRepository(EventsAppDbContext dbContext) : base(dbContext) { }
    }
}
