using System.Text.Json.Serialization;

namespace Events_WEB_APP.Core.Entities
{
    /// <summary>
    /// Класс, представляющий пользователя системы.
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        [JsonPropertyName("userName")]
        public required string UserName { get; set; }

        /// <summary>
        /// Электронная почта пользователя.
        /// </summary>
        [JsonPropertyName("email")]
        public required string Email { get; set; }

        /// <summary>
        /// Хеш пароля пользователя. Игнорируется при сериализации.
        /// </summary>
        [JsonIgnore]
        public required string PasswordHash { get; set; }

        /// <summary>
        /// Идентификатор роли пользователя.
        /// </summary>
        public required Guid RoleId { get; set; }

        /// <summary>
        /// Связанная роль пользователя.
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// Список участников, связанных с пользователем.
        /// </summary>
        public List<Participant>? Participants { get; set; }

        /// <summary>
        /// Токен обновления для пользователя.
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Дата и время истечения срока действия токена обновления.
        /// </summary>
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
