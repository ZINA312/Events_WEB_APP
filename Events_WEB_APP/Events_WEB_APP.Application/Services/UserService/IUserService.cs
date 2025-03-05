using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts.Auth;

namespace Events_WEB_APP.Application.Services.UserService
{
    /// <summary>
    /// Интерфейс для сервиса управления пользователями.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Регистрирует нового пользователя.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="email">Электронная почта пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        Task RegisterAsync(string userName, string email,
            string password);

        /// <summary>
        /// Выполняет вход пользователя.
        /// </summary>
        /// <param name="email">Электронная почта пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Ответ с информацией о аутентификации.</returns>
        Task<AuthResponse> LoginAsync(string email, string password);

        /// <summary>
        /// Получает пользователя по идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <returns>Найденный пользователь.</returns>
        Task<User> GetUserByIdAsync(Guid userId);

        /// <summary>
        /// Обновляет токены доступа и обновления.
        /// </summary>
        /// <param name="accessToken">Токен доступа.</param>
        /// <param name="refreshToken">Токен обновления.</param>
        /// <returns>Ответ с обновленными токенами.</returns>
        Task<AuthResponse> RefreshTokenAsync(string accessToken, string refreshToken);

        /// <summary>
        /// Удаляет пользователя по идентификатору.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя для удаления.</param>
        Task DeleteUserAsync(Guid userId);

        /// <summary>
        /// Получает всех пользователей.
        /// </summary>
        /// <returns>Список всех пользователей.</returns>
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
