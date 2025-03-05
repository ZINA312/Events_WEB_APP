using Events_WEB_APP.Core.Entities;

namespace Events_WEB_APP.Persistence.Repositories.EventRepository
{
    /// <summary>
    /// Интерфейс репозитория для управления событиями.
    /// </summary>
    public interface IEventRepository : IRepository<Event>
    {
    }
}
