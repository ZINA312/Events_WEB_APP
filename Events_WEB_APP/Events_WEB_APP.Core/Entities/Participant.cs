using System.Text.Json.Serialization;

namespace Events_WEB_APP.Core.Entities
{
    /// <summary>
    /// Класс, представляющий участника события.
    /// </summary>
    public class Participant : BaseEntity
    {
        /// <summary>
        /// Идентификатор события, к которому принадлежит участник.
        /// </summary>
        [JsonPropertyName("eventId")]
        public required Guid EventId { get; set; }

        /// <summary>
        /// Идентификатор пользователя, который зарегистрирован как участник.
        /// </summary>
        [JsonPropertyName("userId")]
        public required Guid UserId { get; set; }

        /// <summary>
        /// Связанный пользователь. Игнорируется при сериализации.
        /// </summary>
        [JsonIgnore]
        public User User { get; set; }

        /// <summary>
        /// Имя участника.
        /// </summary>
        [JsonPropertyName("firstName")]
        public required string FirstName { get; set; }

        /// <summary>
        /// Фамилия участника.
        /// </summary>
        [JsonPropertyName("lastName")]
        public required string LastName { get; set; }

        /// <summary>
        /// Дата рождения участника.
        /// </summary>
        [JsonPropertyName("birthDate")]
        public required DateTime BirthDate { get; set; }

        /// <summary>
        /// Дата и время регистрации участника.
        /// </summary>
        [JsonPropertyName("dateTimeOfRegistration")]
        public required DateTime DateTimeOfRegistration { get; set; }

        /// <summary>
        /// Электронная почта участника.
        /// </summary>
        [JsonPropertyName("email")]
        public required string Email { get; set; }
    }
}