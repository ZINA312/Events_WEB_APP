using System.ComponentModel.DataAnnotations;

namespace Events_WEB_APP.Persistence.Contracts.Participant
{
    /// <summary>
    /// Представляет запрос на создание участника, содержащий информацию о пользователе и событии.
    /// </summary>
    /// <param name="UserId">Уникальный идентификатор пользователя (обязательно).</param>
    /// <param name="EventId">Уникальный идентификатор события (обязательно).</param>
    /// <param name="FirstName">Имя участника (обязательно, до 20 символов).</param>
    /// <param name="LastName">Фамилия участника (обязательно, до 20 символов).</param>
    /// <param name="Email">Электронная почта участника (необязательно, должен быть действительным адресом).</param>
    /// <param name="BirthDate">Дата рождения участника (обязательно).</param>
    public record ParticipantCreateRequest(
    [Required] Guid UserId,
    [Required] Guid EventId,
    [Required][StringLength(20)] string FirstName,
    [Required][StringLength(20)] string LastName,
    [EmailAddress] string Email,
    [Required] DateTime BirthDate);
}
