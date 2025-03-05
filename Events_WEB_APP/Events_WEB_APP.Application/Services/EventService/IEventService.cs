using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts;

namespace Events_WEB_APP.Application.Services.EventService
{
    /// <summary>
    /// Интерфейс для сервиса управления событиями.
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Получает событие по идентификатору.
        /// </summary>
        /// <param name="eventId">Идентификатор события.</param>
        /// <returns>Найденное событие.</returns>
        Task<Event> GetEventByIdAsync(Guid eventId);
        /// <summary>
        /// Получает все события.
        /// </summary>
        /// <returns>Список всех событий.</returns>
        Task<List<Event>> GetAllEventsAsync();
        /// <summary>
        /// Получает события по названию с пагинацией.
        /// </summary>
        /// <param name="name">Название события.</param>
        /// <param name="page">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Пагинированный ответ с событиями.</returns>
        Task<PaginatedResponse<Event>> SearchEventsByNameAsync(string name, int page, int pageSize);
        /// <summary>
        /// Получает события с пагинацией.
        /// </summary>
        /// <param name="categoryName">Название категории для фильтрации.</param>
        /// <param name="date">Дата события для фильтрации.</param>
        /// <param name="location">Место проведения события для фильтрации.</param>
        /// <param name="pageNo">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Пагинированный ответ с событиями.</returns>
        Task<PaginatedResponse<Event>> GetEventsPaginatedAsync(string? categoryName,
            DateTime? date,
            string? location,
            int pageNo = 1,
            int pageSize = 10);
        /// <summary>
        /// Создает новое событие.
        /// </summary>
        /// <param name="eventEntity">Событие для создания.</param>
        Task CreateEventAsync(Event eventEntity);
        /// <summary>
        /// Обновляет существующее событие.
        /// </summary>
        /// <param name="eventEntity">Обновленное событие.</param>
        Task UpdateEventAsync(Event eventEntity);
        /// <summary>
        /// Удаляет событие по идентификатору.
        /// </summary>
        /// <param name="eventId">Идентификатор события для удаления.</param>
        Task DeleteEventAsync(Guid eventId);
    }
}
