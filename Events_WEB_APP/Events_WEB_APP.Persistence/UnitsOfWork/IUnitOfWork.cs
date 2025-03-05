using Events_WEB_APP.Persistence.Repositories.CategoryRepository;
using Events_WEB_APP.Persistence.Repositories.EventRepository;
using Events_WEB_APP.Persistence.Repositories.ParticipantRepository;
using Events_WEB_APP.Persistence.Repositories.RoleRepository;
using Events_WEB_APP.Persistence.Repositories.UserRepository;

namespace Events_WEB_APP.Persistence.UnitsOfWork
{
    /// <summary>
    /// Интерфейс единицы работы для управления репозиториями.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IEventRepository Events { get; }
        IParticipantRepository Participants { get; }
        IRoleRepository Roles { get; }
        IUserRepository Users { get; }
        Task CommitAsync();
    }
}
