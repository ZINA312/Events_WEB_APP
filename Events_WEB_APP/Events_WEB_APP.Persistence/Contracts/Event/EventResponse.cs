namespace Events_WEB_APP.Persistence.Contracts.Event
{
    /// <summary>
    /// Представляет ответ для события, содержащий информацию о событии.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор события.</param>
    /// <param name="Name">Название события.</param>
    /// <param name="Description">Описание события.</param>
    /// <param name="Date">Дата события.</param>
    /// <param name="Location">Место проведения события.</param>
    /// <param name="CategoryId">Идентификатор категории события.</param>
    /// <param name="MaxNumOfParticipants">Максимальное количество участников.</param>
    /// <param name="ImageUrl">URL изображения события.</param>
    public record EventResponse(
    Guid Id,
    string Name,
    string Description,
    DateTime Date,
    string Location,
    Guid CategoryId,
    int MaxNumOfParticipants,
    string ImageUrl)
    {
        /// <summary>
        /// Конструктор по умолчанию для создания нового экземпляра <see cref="EventResponse"/>.
        /// </summary>
        public EventResponse() : this(
            default,
            string.Empty,
            string.Empty,
            default,
            string.Empty,
            default,
            default,
            string.Empty)
        {
        }
    }
}
