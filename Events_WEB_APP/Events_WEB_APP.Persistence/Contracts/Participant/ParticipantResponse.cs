
namespace Events_WEB_APP.Persistence.Contracts.Participant
{
    public record ParticipantResponse(
    Guid Id,
    Guid UserId,
    Guid EventId,
    string FirstName,
    string LastName,
    string Email,
    DateTime BirthDate);
}
