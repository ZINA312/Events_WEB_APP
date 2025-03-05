using Events_WEB_APP.Core.Entities;

namespace Events_WEB_APP.Application.Services.RoleService
{
    /// <summary>
    /// Интерфейс для сервиса управления ролями.
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Создает новую роль.
        /// </summary>
        /// <param name="role">Роль для создания.</param>
        /// <returns>Созданная роль.</returns>
        Task<Role> CreateRoleAsync(Role role);

        /// <summary>
        /// Удаляет роль по идентификатору.
        /// </summary>
        /// <param name="roleId">Идентификатор роли для удаления.</param>
        Task DeleteRoleAsync(Guid roleId);

        /// <summary>
        /// Получает все роли.
        /// </summary>
        /// <returns>Список всех ролей.</returns>
        Task<IEnumerable<Role>> GetAllRolesAsync();

        /// <summary>
        /// Получает роль по идентификатору.
        /// </summary>
        /// <param name="roleId">Идентификатор роли.</param>
        /// <returns>Найденная роль.</returns>
        Task<Role> GetRoleByIdAsync(Guid roleId);

        /// <summary>
        /// Обновляет существующую роль.
        /// </summary>
        /// <param name="roleId">Идентификатор роли для обновления.</param>
        /// <param name="newRoleName">Новое имя роли.</param>
        /// <returns>Обновленная роль.</returns>
        Task<Role> UpdateRoleAsync(Guid roleId, string newRoleName);
    }
}
