using System.Text.Json.Serialization;

namespace Events_WEB_APP.Core.Entities
{
    public class Category : BaseEntity
    {
        [JsonPropertyName("name")]
        public required string Name {  get; set; }
    }
}
