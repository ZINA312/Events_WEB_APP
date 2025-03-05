using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts;
using Events_WEB_APP.Persistence.UnitsOfWork;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Events_WEB_APP.Application.Services.EventService
{
    /// <summary>
    /// Сервис для управления событиями.
    /// </summary>
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Event> _validator;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EventService"/>.
        /// </summary>
        /// <param name="unitOfWork">Единица работы для доступа к репозиториям.</param>
        /// <param name="validator">Валидатор для проверки событий.</param>
        public EventService(IUnitOfWork unitOfWork, IValidator<Event> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator; 
        }

        /// <summary>
        /// Создает новое событие.
        /// </summary>
        /// <param name="eventEntity">Событие для создания.</param>
        public async Task CreateEventAsync(Event eventEntity)
        {
            ArgumentNullException.ThrowIfNull(eventEntity, nameof(eventEntity));
            await ValidateEventAsync(eventEntity);
            var category = await _unitOfWork.Categories.GetByIdAsync(eventEntity.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Category with ID {eventEntity.CategoryId} not found",
                    nameof(eventEntity.CategoryId));
            }
            await _unitOfWork.Events.AddAsync(eventEntity);
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Удаляет событие по идентификатору.
        /// </summary>
        /// <param name="eventId">Идентификатор события для удаления.</param>
        public async Task DeleteEventAsync(Guid eventId)
        {
            if (eventId == Guid.Empty)
            {
                throw new ArgumentException("Invalid event ID", nameof(eventId)); 
            }
            var eventEntity = await _unitOfWork.Events.GetByIdAsync(eventId);
            if (eventEntity == null)
            {
                throw new KeyNotFoundException($"Event with ID {eventId} not found");
            }
            await _unitOfWork.Events.DeleteAsync(eventEntity);
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Получает все события.
        /// </summary>
        /// <returns>Список событий.</returns>
        public async Task<List<Event>> GetAllEventsAsync()
        {
            return (await _unitOfWork.Events.GetAllAsync()).ToList();
        }

        /// <summary>
        /// Получает события по названию с пагинацией.
        /// </summary>
        /// <param name="name">Название события.</param>
        /// <param name="page">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Пагинированный ответ с событиями.</returns>
        public async Task<PaginatedResponse<Event>> SearchEventsByNameAsync(
            string name,
            int page = 1,
            int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Search query is required");

            var query = _unitOfWork.Events.GetAll()
                .Where(e => e.Name.Contains(name));

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(e => e.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<Event>(items, page, pageSize, totalCount);
        }

        /// <summary>
        /// Получает событие по идентификатору.
        /// </summary>
        /// <param name="eventId">Идентификатор события.</param>
        /// <returns>Найденное событие.</returns>
        public async Task<Event> GetEventByIdAsync(Guid eventId)
        {
            if (eventId == Guid.Empty)
                throw new ArgumentException("Invalid event ID", nameof(eventId));

            var eventEntity = await _unitOfWork.Events.GetByIdAsync(eventId);
            return eventEntity ?? throw new KeyNotFoundException($"Event with ID {eventId} not found");
        }

        /// <summary>
        /// Получает события с пагинацией.
        /// </summary>
        /// <param name="categoryName">Название категории для фильтрации.</param>
        /// <param name="date">Дата события для фильтрации.</param>
        /// <param name="location">Место проведения события для фильтрации.</param>
        /// <param name="pageNo">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Пагинированный ответ с событиями.</returns>
        public async Task<PaginatedResponse<Event>> GetEventsPaginatedAsync(
            string? categoryName,
            DateTime? date,
            string? location,
            int pageNo = 1,
            int pageSize = 10)
        {
            if(pageNo < 1)
            {
                throw new ArgumentException("Invalid page number",  nameof(pageNo));
            }
            if (pageSize <= 0)
            {
                throw new ArgumentException("Invalid page size", nameof(pageSize));
            }
            var query = _unitOfWork.Events.GetAll();
            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                var category = await _unitOfWork.Categories
                    .FindAsync(c => c.Name == categoryName.Trim());

                if (category != null)
                    query = query.Where(e => e.CategoryId == category.Id);
            }

            if (date.HasValue)
                query = query.Where(e => e.Date.Date == date.Value.Date);

            if (!string.IsNullOrWhiteSpace(location))
                query = query.Where(e => e.Location.Contains(location));

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderBy(e => e.Date)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResponse<Event>(items, pageNo, pageSize, totalCount);
        }

        /// <summary>
        /// Обновляет существующее событие.
        /// </summary>
        /// <param name="eventEntity">Обновленное событие.</param>
        public async Task UpdateEventAsync(Event eventEntity)
        {
            ArgumentNullException.ThrowIfNull(eventEntity, nameof(eventEntity));

            await ValidateEventAsync(eventEntity); 

            var existingEvent = await _unitOfWork.Events.GetByIdAsync(eventEntity.Id);
            if (existingEvent == null)
                throw new KeyNotFoundException($"Event with ID {eventEntity.Id} not found");

            existingEvent.Name = eventEntity.Name;
            existingEvent.Description = eventEntity.Description;
            existingEvent.Date = eventEntity.Date;
            existingEvent.Location = eventEntity.Location;
            existingEvent.CategoryId = eventEntity.CategoryId;
            existingEvent.MaxNumOfParticipants = eventEntity.MaxNumOfParticipants;
            existingEvent.ImagePath = eventEntity.ImagePath;

            await _unitOfWork.Events.UpdateAsync(existingEvent);
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Валидирует событие.
        /// </summary>
        /// <param name="eventEntity">Событие для валидации.</param>
        private async Task ValidateEventAsync(Event eventEntity)
        {
            var validationResult = await _validator.ValidateAsync(eventEntity);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errors);
            }
        }
    }
}
