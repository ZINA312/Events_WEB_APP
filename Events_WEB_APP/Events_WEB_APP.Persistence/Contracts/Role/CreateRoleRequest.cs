using System.ComponentModel.DataAnnotations;

namespace Events_WEB_APP.Persistence.Contracts.Role
{
    /// <summary>
    /// Представляет запрос на создание роли, содержащий имя роли.
    /// </summary>
    /// <param name="Name">Имя роли (обязательно, не более 50 символов).</param>
    public record CreateRoleRequest(
        [Required(ErrorMessage = "Role name is required")]
        [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters")]
        string Name
    );
}
