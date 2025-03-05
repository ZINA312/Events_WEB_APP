using System.ComponentModel.DataAnnotations;

namespace Events_WEB_APP.Persistence.Contracts.User
{
    /// <summary>
    /// Представляет запрос на регистрацию пользователя, содержащий необходимые данные.
    /// </summary>
    /// <param name="UserName">Имя пользователя (обязательно, не более 50 символов).</param>
    /// <param name="Email">Электронная почта пользователя (обязательно, должен быть действительным адресом).</param>
    /// <param name="Password">Пароль пользователя (обязательно, от 6 до 100 символов).</param>
    public record RegisterUserRequest(
    [Required][StringLength(50)] string UserName,
    [Required][EmailAddress] string Email,
    [Required][StringLength(100, MinimumLength = 6)] string Password);
}
