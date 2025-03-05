using System.Text.Json.Serialization;

namespace Events_WEB_APP.Core.Entities
{
    /// <summary>
    /// Класс, представляющий роль пользователя.
    /// </summary>
    public class Role : BaseEntity
    {
        /// <summary>
        /// Название роли.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
