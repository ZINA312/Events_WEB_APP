using System.Text.Json.Serialization;

namespace Events_WEB_APP.Core.Entities
{
    public class Role : BaseEntity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
