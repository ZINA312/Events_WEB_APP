using Events_WEB_APP.Persistence.Data;
using Events_WEB_APP.Persistence.Repositories.CategoryRepository;
using Events_WEB_APP.Persistence.Repositories.EventRepository;
using Events_WEB_APP.Persistence.Repositories.ParticipantRepository;
using Events_WEB_APP.Persistence.Repositories.RoleRepository;
using Events_WEB_APP.Persistence.Repositories.UserRepository;

namespace Events_WEB_APP.Persistence.UnitsOfWork
{
    /// <summary>
    /// Реализация единицы работы для управления репозиториями.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EventsAppDbContext _context;
        private ICategoryRepository _categoryRepository;
        private IEventRepository _eventRepository;
        private IParticipantRepository _participantRepository;
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="UnitOfWork"/>.
        /// </summary>
        /// <param name="context">Контекст базы данных.</param>
        /// <param name="categoryRepository">Репозиторий категорий.</param>
        /// <param name="eventRepository">Репозиторий событий.</param>
        /// <param name="participantRepository">Репозиторий участников.</param>
        /// <param name="roleRepository">Репозиторий ролей.</param>
        /// <param name="userRepository">Репозиторий пользователей.</param>
        public UnitOfWork(EventsAppDbContext context, ICategoryRepository categoryRepository, IEventRepository eventRepository,
            IParticipantRepository participantRepository, IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _context = context;
            _categoryRepository = categoryRepository;
            _eventRepository = eventRepository;
            _participantRepository = participantRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public ICategoryRepository Categories
        {
            get { return _categoryRepository; }
        }
        public IEventRepository Events
        {
            get { return _eventRepository; }
        }
        public IParticipantRepository Participants
        {
            get { return _participantRepository; }
        }
        public IRoleRepository Roles
        {
            get { return _roleRepository; }
        }
        public IUserRepository Users
        {
            get { return _userRepository; }
        }

        /// <summary>
        /// Асинхронно сохраняет изменения в базе данных.
        /// </summary>
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Освобождает ресурсы, используемые единицей работы.
        /// </summary>
        public async void Dispose()
        {
            await _context.DisposeAsync();
        }
    }
}
