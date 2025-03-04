using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Events_WEB_APP.Persistence.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly EventsAppDbContext _context;
        protected DbSet<T> _entities;

        public Repository(EventsAppDbContext dbContext)
        {
            _context = dbContext;
            _entities = _context.Set<T>();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.AsNoTracking().ToListAsync();
        }

        public IQueryable<Event> GetAll()
        {
            return _context.Events.AsQueryable();
        }

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _entities.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task DeleteAsync(T entity)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity);

                _entities.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.FirstOrDefaultAsync(predicate);
        }
    }
}
