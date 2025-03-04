using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts;
using Events_WEB_APP.Persistence.UnitsOfWork;
using Microsoft.EntityFrameworkCore;

namespace Events_WEB_APP.Application.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateEventAsync(Event eventEntity)
        {
            ArgumentNullException.ThrowIfNull(eventEntity, nameof(eventEntity));
            ValidateEvent(eventEntity);
            var category = await _unitOfWork.Categories.GetByIdAsync(eventEntity.CategoryId);
            if (category == null)
            {
                throw new ArgumentException($"Category with ID {eventEntity.CategoryId} not found",
                    nameof(eventEntity.CategoryId));
            }
            await _unitOfWork.Events.AddAsync(eventEntity);
            await _unitOfWork.CommitAsync();
        }

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

        public async Task<List<Event>> GetAllEventsAsync()
        {
            return (await _unitOfWork.Events.GetAllAsync()).ToList();
        }

        public async Task<Event> GetEventByIdAsync(Guid eventId)
        {
            if (eventId == Guid.Empty)
                throw new ArgumentException("Invalid event ID", nameof(eventId));

            var eventEntity = await _unitOfWork.Events.GetByIdAsync(eventId);
            return eventEntity ?? throw new KeyNotFoundException($"Event with ID {eventId} not found");
        }

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

        public async Task UpdateEventAsync(Event eventEntity)
        {
            ArgumentNullException.ThrowIfNull(eventEntity, nameof(eventEntity));

            ValidateEvent(eventEntity); 

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

        private void ValidateEvent(Event eventEntity)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(eventEntity.Name))
                errors.Add("Event name is required");
            else if (eventEntity.Name.Length > 100)
                errors.Add("Event name must be 100 characters or less");

            if (!string.IsNullOrWhiteSpace(eventEntity.Description))
            {
                if (eventEntity.Description.Length > 500)
                    errors.Add("Description must be 500 characters or less");
            }

            if (eventEntity.Date < DateTime.Now)
                errors.Add("Event date cannot be in the past");

            if (string.IsNullOrWhiteSpace(eventEntity.Location))
                errors.Add("Location is required");
            else if (eventEntity.Location.Length > 50)
                errors.Add("Location must be 50 characters or less");

            if (eventEntity.MaxNumOfParticipants <= 0)
                errors.Add("Max participants must be greater than 0");

            eventEntity.ImagePath ??= "default.png";
            if (string.IsNullOrWhiteSpace(eventEntity.ImagePath))
                errors.Add("Image path is required");
            else if (eventEntity.ImagePath.Length > 50)
                errors.Add("Image path too long (max 50 characters)");

            if (errors.Any())
                throw new ArgumentException(string.Join("\n", errors));
        }
    }
}
