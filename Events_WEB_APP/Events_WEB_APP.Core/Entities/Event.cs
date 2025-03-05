using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Events_WEB_APP.Core.Entities
{
    /// <summary>
    /// Класс, представляющий событие.
    /// </summary>
    public class Event : BaseEntity
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Event"/>.
        /// </summary>
        public Event() { }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="Event"/> с указанными параметрами.
        /// </summary>
        /// <param name="id">Уникальный идентификатор события.</param>
        /// <param name="name">Название события.</param>
        /// <param name="description">Описание события.</param>
        /// <param name="date">Дата события.</param>
        /// <param name="location">Место проведения события.</param>
        /// <param name="categoryId">Идентификатор категории события.</param>
        /// <param name="maxParticipants">Максимальное количество участников.</param>
        /// <param name="imgPath">Путь к изображению события.</param>
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

        /// <summary>
        /// Название события.
        /// </summary>
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        /// <summary>
        /// Описание события.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Дата события.
        /// </summary>
        [JsonPropertyName("date")]
        public required DateTime Date { get; set; }

        /// <summary>
        /// Место проведения события.
        /// </summary>
        [JsonPropertyName("location")]
        public required string Location { get; set; }

        /// <summary>
        /// Идентификатор категории события.
        /// </summary>
        [JsonPropertyName("categoryId")]
        public required Guid CategoryId { get; set; }

        /// <summary>
        /// Категория события.
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Максимальное количество участников.
        /// </summary>
        [JsonPropertyName("maxNumOfParticipants")]
        public int MaxNumOfParticipants { get; set; }

        /// <summary>
        /// Путь к изображению события.
        /// </summary>
        [JsonPropertyName("imagePath")]
        public string? ImagePath { get; set; }
    }
}
