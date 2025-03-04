using System.Text.Json.Serialization;

namespace Events_WEB_APP.Core.Entities
{
    public class BaseEntity
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }
}
