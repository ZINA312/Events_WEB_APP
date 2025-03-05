using System.Text.Json.Serialization;

namespace Events_WEB_APP.Core.Entities
{
    /// <summary>
    /// Базовый класс для всех сущностей.
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Уникальный идентификатор сущности.
        /// </summary>
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}
