namespace Events_WEB_APP.Persistence.Contracts.User
{
    /// <summary>
    /// Представляет ответ с информацией о пользователе.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор пользователя.</param>
    /// <param name="UserName">Имя пользователя.</param>
    /// <param name="Email">Электронная почта пользователя.</param>
    /// <param name="RoleId">Уникальный идентификатор роли пользователя.</param>
    public record UserResponse(
    Guid Id,
    string UserName,
    string Email,
    Guid RoleId);
}
