
using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts.Participant;

namespace Events_WEB_APP.Application.Services.ParticipantService
{
    public interface IParticipantService
    {
        Task<Participant> CreateParticipantAsync(Participant participant);
        Task<Participant> DeleteParticipantAsync(Guid participantId);
        Task<List<Participant>> GetAllParticipantsAsync();
        Task<Participant> GetParticipantByIdAsync(Guid participantId);
        Task<List<Participant>> GetParticipantsByEventIdPaginatedAsync(Guid? eventId, int pageNo = 1, int pageSize = 10);
        Task<List<Participant>> GetParticipantsByUserIdPaginatedAsync(Guid? userId, int pageNo = 1, int pageSize = 10);
        Task<Participant> UpdateParticipantAsync(Participant participantEntity);
    }
}
