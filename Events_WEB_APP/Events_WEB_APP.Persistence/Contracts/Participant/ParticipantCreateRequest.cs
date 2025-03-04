using System.ComponentModel.DataAnnotations;

namespace Events_WEB_APP.Persistence.Contracts.Participant
{
    public record ParticipantCreateRequest(
    [Required] Guid UserId,
    [Required] Guid EventId,
    [Required][StringLength(20)] string FirstName,
    [Required][StringLength(20)] string LastName,
    [EmailAddress] string Email,
    [Required] DateTime BirthDate);
}
