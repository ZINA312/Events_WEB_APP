using Events_WEB_APP.Core.Entities;
using System.Linq.Expressions;

namespace Events_WEB_APP.Persistence.Repositories
{
    public interface IRepository<T> : IDisposable where T : BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        IQueryable<Event> GetAll();
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
