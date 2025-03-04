using System.ComponentModel.DataAnnotations;

namespace Events_WEB_APP.Persistence.Contracts.Event
{
    public record EventUpdateRequest(
    Guid Id,
    [Required][StringLength(100)] string Name,
    [StringLength(500)] string Description,
    DateTime Date,
    [Required][StringLength(50)] string Location,
    Guid CategoryId,
    [Range(1, 1000)] int MaxNumOfParticipants);
}
