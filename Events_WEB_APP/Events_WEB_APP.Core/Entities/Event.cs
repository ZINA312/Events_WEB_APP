using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Events_WEB_APP.Core.Entities
{
    public class Event : BaseEntity
    {
        public Event() { }

        public Event(Guid id, string name, string description, DateTime date,
            string location, Guid categoryId, int maxParticipants, string? imgPath) 
        {
            Id = id;
            Name = name;
            Description = description;
            Date = date;
            Location = location;
            CategoryId = categoryId;
            MaxNumOfParticipants = maxParticipants;
            ImagePath = imgPath;
        }

        [JsonPropertyName("name")]
        public required string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("date")]
        public required DateTime Date { get; set; }
        [JsonPropertyName("location")]
        public required string Location { get; set; }
        [JsonPropertyName("categoryId")]
        public required Guid CategoryId { get; set; }
        public Category Category { get; set; }
        [JsonPropertyName("maxNumOfParticipants")]
        public int MaxNumOfParticipants { get; set; }
        [JsonPropertyName("imagePath")]
        public string? ImagePath { get; set; }
    }
}
