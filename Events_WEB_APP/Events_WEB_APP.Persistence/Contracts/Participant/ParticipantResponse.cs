
namespace Events_WEB_APP.Persistence.Contracts.Participant
{
    /// <summary>
    /// Представляет ответ для участника, содержащий информацию о его участии в событии.
    /// </summary>
    /// <param name="Id">Уникальный идентификатор участника.</param>
    /// <param name="UserId">Уникальный идентификатор пользователя.</param>
    /// <param name="EventId">Уникальный идентификатор события, в котором участвует участник.</param>
    /// <param name="FirstName">Имя участника.</param>
    /// <param name="LastName">Фамилия участника.</param>
    /// <param name="Email">Электронная почта участника.</param>
    /// <param name="BirthDate">Дата рождения участника.</param>
    public record ParticipantResponse(
    Guid Id,
    Guid UserId,
    Guid EventId,
    string FirstName,
    string LastName,
    string Email,
    DateTime BirthDate);
}
