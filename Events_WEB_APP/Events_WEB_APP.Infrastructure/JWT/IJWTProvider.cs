using Events_WEB_APP.Core.Entities;
using System.Security.Claims;

namespace Events_WEB_APP.Infrastructure.JWT
{
    /// <summary>
    /// Интерфейс для работы с JWT (JSON Web Tokens).
    /// </summary>
    public interface IJWTProvider
    {
        /// <summary>
        /// Генерирует JWT для указанного пользователя.
        /// </summary>
        /// <param name="user">Пользователь, для которого генерируется токен.</param>
        /// <returns>Сгенерированный токен.</returns>
        string GenerateToken(User user);

        /// <summary>
        /// Генерирует токен обновления.
        /// </summary>
        /// <returns>Сгенерированный токен обновления.</returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Извлекает объект <see cref="ClaimsPrincipal"/> из токена.
        /// </summary>
        /// <param name="token">JWT, из которого будет извлечен объект.</param>
        /// <returns>Объект <see cref="ClaimsPrincipal"/> из токена.</returns>
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}