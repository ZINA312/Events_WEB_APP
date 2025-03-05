using System.Text.Json.Serialization;

namespace Events_WEB_APP.Core.Entities
{
    /// <summary>
    /// Класс, представляющий категорию события.
    /// </summary>
    public class Category : BaseEntity
    {
        /// <summary>
        /// Название категории.
        /// </summary>
        [JsonPropertyName("name")]
        public required string Name {  get; set; }
    }
}
