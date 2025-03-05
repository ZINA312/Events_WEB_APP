using System.ComponentModel.DataAnnotations;

namespace Events_WEB_APP.Persistence.Contracts.Event
{
    /// <summary>
    /// Представляет запрос на создание события, содержащий необходимую информацию о событии.
    /// </summary>
    /// <param name="Name">Название события (обязательно, до 100 символов).</param>
    /// <param name="Description">Описание события (до 500 символов).</param>
    /// <param name="Date">Дата события.</param>
    /// <param name="Location">Местоположение события (обязательно, до 50 символов).</param>
    /// <param name="CategoryId">Уникальный идентификатор категории события.</param>
    /// <param name="MaxNumOfParticipants">Максимальное количество участников (от 1 до 1000).</param>
    public record EventCreateRequest(
    [Required][StringLength(100)] string Name,
    [StringLength(500)] string Description,
    DateTime Date,
    [Required][StringLength(50)] string Location,
    Guid CategoryId,
    [Range(1, 1000)] int MaxNumOfParticipants);
}
