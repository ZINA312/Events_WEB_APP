using System.ComponentModel.DataAnnotations;

namespace Events_WEB_APP.Persistence.Contracts.Category
{
    /// <summary>
    /// Представляет запрос на обновление категории, содержащий идентификатор и имя категории.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор категории.</param>
    /// <param name="Name">Новое имя категории, которое должно быть от 1 до 30 символов.</param>
    public record UpdateCategoryRequest(
    Guid Id,
    [Required][StringLength(30, MinimumLength = 1)] string Name);
}
