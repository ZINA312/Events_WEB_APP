using Events_WEB_APP.Core.Entities;
using System.Linq.Expressions;

namespace Events_WEB_APP.Persistence.Repositories
{
    /// <summary>
    /// Интерфейс общего репозитория для работы с сущностями.
    /// </summary>
    /// <typeparam name="T">Тип сущности, который должен наследовать <see cref="BaseEntity"/>.</typeparam>
    public interface IRepository<T> : IDisposable where T : BaseEntity
    {
        /// <summary>
        /// Асинхронно получает сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>Сущность с указанным идентификатором.</returns>
        Task<T> GetByIdAsync(Guid id);

        /// <summary>
        /// Асинхронно получает все сущности.
        /// </summary>
        /// <returns>Список всех сущностей.</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Получает все сущности в виде IQueryable.
        /// </summary>
        /// <returns>Запрос для получения всех сущностей.</returns>
        IQueryable<Event> GetAll();

        /// <summary>
        /// Асинхронно получает сущности по заданному условию.
        /// </summary>
        /// <param name="predicate">Условие для фильтрации сущностей.</param>
        /// <returns>Список сущностей, соответствующих условию.</returns>
        Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Асинхронно находит сущность по заданному условию.
        /// </summary>
        /// <param name="predicate">Условие для поиска сущности.</param>
        /// <returns>Сущность, соответствующая условию, или null, если не найдено.</returns>
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Асинхронно добавляет новую сущность.
        /// </summary>
        /// <param name="entity">Сущность для добавления.</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Асинхронно обновляет существующую сущность.
        /// </summary>
        /// <param name="entity">Сущность для обновления.</param>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Асинхронно удаляет сущность.
        /// </summary>
        /// <param name="entity">Сущность для удаления.</param>
        Task DeleteAsync(T entity);
    }
}
