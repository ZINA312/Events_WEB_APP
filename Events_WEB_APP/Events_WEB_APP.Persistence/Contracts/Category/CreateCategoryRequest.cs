using System.ComponentModel.DataAnnotations;

namespace Events_WEB_APP.Persistence.Contracts.Category
{
    /// <summary>
    /// Представляет запрос на создание категории, содержащий имя категории.
    /// </summary>
    /// <param name="Name">Имя категории, которое должно быть от 1 до 30 символов.</param>
    public record CreateCategoryRequest(
    [Required][StringLength(30, MinimumLength = 1)] string Name);
}
