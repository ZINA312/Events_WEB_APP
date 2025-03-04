using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts;

namespace Events_WEB_APP.Application.Services.EventService
{
    public interface IEventService
    {
        Task<Event> GetEventByIdAsync(Guid eventId);
        Task<List<Event>> GetAllEventsAsync();
        Task<PaginatedResponse<Event>> GetEventsPaginatedAsync(string? categoryName,
            DateTime? date,
            string? location,
            int pageNo = 1,
            int pageSize = 10);
        Task CreateEventAsync(Event eventEntity);
        Task UpdateEventAsync(Event eventEntity);
        Task DeleteEventAsync(Guid eventId);
    }
}
