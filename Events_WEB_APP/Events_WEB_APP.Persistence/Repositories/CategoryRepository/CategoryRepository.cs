using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Data;

namespace Events_WEB_APP.Persistence.Repositories.CategoryRepository
{
    /// <summary>
    /// Репозиторий для управления категориями в базе данных.
    /// </summary>
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CategoryRepository"/>.
        /// </summary>
        /// <param name="dbContext">Контекст базы данных для доступа к данным.</param>
        public CategoryRepository(EventsAppDbContext dbContext) : base(dbContext) { }
    }
}
