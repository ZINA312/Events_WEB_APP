using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Data;

namespace Events_WEB_APP.Persistence.Repositories.CategoryRepository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(EventsAppDbContext dbContext) : base(dbContext) { }
    }
}
