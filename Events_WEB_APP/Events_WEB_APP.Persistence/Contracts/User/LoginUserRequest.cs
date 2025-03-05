using System.ComponentModel.DataAnnotations;

namespace Events_WEB_APP.Persistence.Contracts.User
{
    /// <summary>
    /// Представляет запрос на вход пользователя, содержащий учетные данные для авторизации.
    /// </summary>
    /// <param name="Email">Электронная почта пользователя (обязательно, должен быть действительным адресом).</param>
    /// <param name="Password">Пароль пользователя (обязательно).</param>
    public record LoginUserRequest(
    [Required][EmailAddress] string Email,
    [Required] string Password);
}
