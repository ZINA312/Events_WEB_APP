using System.Text.Json.Serialization;

namespace Events_WEB_APP.Core.Entities
{
    public class Participant : BaseEntity
    {
        [JsonPropertyName("eventId")]
        public required Guid EventId { get; set; }
        [JsonPropertyName("userId")]
        public required Guid UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [JsonPropertyName("firstName")]
        public required string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public required string LastName { get; set; }
        [JsonPropertyName("birthDate")]
        public required DateTime BirthDate { get; set; }
        [JsonPropertyName("dateTimeOfRegistration")]
        public required DateTime DateTimeOfRegistration { get; set; }
        [JsonPropertyName("email")]
        public required string Email { get; set; }
    }
}