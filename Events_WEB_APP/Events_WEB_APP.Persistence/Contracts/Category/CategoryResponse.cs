namespace Events_WEB_APP.Persistence.Contracts.Category
{
    /// <summary>
    /// Представляет ответ для категории, содержащий идентификатор и имя категории.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор категории.</param>
    /// <param name="Name">Имя категории.</param>
    public record CategoryResponse(
    Guid Id,
    string Name);
}
