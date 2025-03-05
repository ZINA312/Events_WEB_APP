using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.UnitsOfWork;
using FluentValidation;

namespace Events_WEB_APP.Application.Services.RoleService
{
    /// <summary>
    /// Сервис для управления ролями.
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Role> _validator;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="RoleService"/>.
        /// </summary>
        /// <param name="unitOfWork">Единица работы для доступа к репозиториям.</param>
        /// <param name="validator">Валидатор для проверки ролей.</param>
        public RoleService(IUnitOfWork unitOfWork, IValidator<Role> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        /// <summary>
        /// Создает новую роль.
        /// </summary>
        /// <param name="role">Роль для создания.</param>
        /// <returns>Созданная роль.</returns>
        public async Task<Role> CreateRoleAsync(Role role)
        {
            await ValidateRoleAsync(role);

            await _unitOfWork.Roles.AddAsync(role);
            await _unitOfWork.CommitAsync();
            return role;
        }

        /// <summary>
        /// Удаляет роль по идентификатору.
        /// </summary>
        /// <param name="roleId">Идентификатор роли для удаления.</param>
        public async Task DeleteRoleAsync(Guid roleId)
        {
            var role = await ValidateRoleExists(roleId);
            await _unitOfWork.Roles.DeleteAsync(role);
            await _unitOfWork.CommitAsync();
        }

        /// <summary>
        /// Получает все роли.
        /// </summary>
        /// <returns>Список всех ролей.</returns>
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _unitOfWork.Roles.GetAllAsync();
        }

        /// <summary>
        /// Получает роль по идентификатору.
        /// </summary>
        /// <param name="roleId">Идентификатор роли.</param>
        /// <returns>Найденная роль.</returns>
        public async Task<Role> GetRoleByIdAsync(Guid roleId)
        {
            return await ValidateRoleExists(roleId);
        }

        /// <summary>
        /// Обновляет существующую роль.
        /// </summary>
        /// <param name="roleId">Идентификатор роли для обновления.</param>
        /// <param name="newRoleName">Новое имя роли.</param>
        /// <returns>Обновленная роль.</returns>
        public async Task<Role> UpdateRoleAsync(Guid roleId, string newRoleName)
        {
            var role = await ValidateRoleExists(roleId);
            role.Name = newRoleName;
            await ValidateRoleAsync(role);
            await _unitOfWork.Roles.UpdateAsync(role);
            await _unitOfWork.CommitAsync();
            return role;
        }

        /// <summary>
        /// Проверяет существование роли по идентификатору.
        /// </summary>
        /// <param name="roleId">Идентификатор роли.</param>
        /// <returns>Найденная роль.</returns>
        /// <exception cref="KeyNotFoundException">Исключение, если роль не найдена.</exception>
        private async Task<Role> ValidateRoleExists(Guid roleId)
        {
            var role = await _unitOfWork.Roles.GetByIdAsync(roleId);
            return role ?? throw new KeyNotFoundException($"Role with ID {roleId} not found");
        }

        /// <summary>
        /// Валидирует роль.
        /// </summary>
        /// <param name="role">Роль для валидации.</param>
        /// <exception cref="ArgumentException">Исключение, если роль недействительна.</exception>
        private async Task ValidateRoleAsync(Role role)
        {
            var validationResult = await _validator.ValidateAsync(role);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errors);
            }
        }
    }
}