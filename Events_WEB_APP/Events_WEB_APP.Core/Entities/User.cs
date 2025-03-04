using System.Text.Json.Serialization;

namespace Events_WEB_APP.Core.Entities
{
    public class User : BaseEntity
    {
        [JsonPropertyName("userName")]
        public required string UserName { get; set; }
        [JsonPropertyName("email")]
        public required string Email { get; set; }
        [JsonIgnore]
        public required string PasswordHash { get; set; }
        public required Guid RoleId { get; set; }
        public Role Role { get; set; }
        public List<Participant>? Participants { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
