using Events_WEB_APP.Core.Entities;

namespace Events_WEB_APP.Application.Services.ParticipantService
{
    /// <summary>
    /// Интерфейс для сервиса управления участниками.
    /// </summary>
    public interface IParticipantService
    {
        /// <summary>
        /// Создает нового участника.
        /// </summary>
        /// <param name="participant">Участник для создания.</param>
        /// <returns>Созданный участник.</returns>
        Task<Participant> CreateParticipantAsync(Participant participant);
        /// <summary>
        /// Удаляет участника по идентификатору.
        /// </summary>
        /// <param name="participantId">Идентификатор участника для удаления.</param>
        /// <returns>Удаленный участник.</returns>
        Task<Participant> DeleteParticipantAsync(Guid participantId);
        /// <summary>
        /// Получает всех участников.
        /// </summary>
        /// <returns>Список всех участников.</returns>
        Task<List<Participant>> GetAllParticipantsAsync();
        /// <summary>
        /// Получает участника по идентификатору.
        /// </summary>
        /// <param name="participantId">Идентификатор участника.</param>
        /// <returns>Найденный участник.</returns>
        Task<Participant> GetParticipantByIdAsync(Guid participantId);
        /// <summary>
        /// Получает участников по идентификатору события с пагинацией.
        /// </summary>
        /// <param name="eventId">Идентификатор события.</param>
        /// <param name="pageNo">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Список участников события с пагинацией.</returns>
        Task<List<Participant>> GetParticipantsByEventIdPaginatedAsync(Guid? eventId, int pageNo = 1, int pageSize = 10);
        /// <summary>
        /// Получает участников по идентификатору пользователя с пагинацией.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="pageNo">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Список участников пользователя с пагинацией.</returns>
        Task<List<Participant>> GetParticipantsByUserIdPaginatedAsync(Guid? userId, int pageNo = 1, int pageSize = 10);
        /// <summary>
        /// Обновляет существующего участника.
        /// </summary>
        /// <param name="participantEntity">Обновленный участник.</param>
        /// <returns>Обновленный участник.</returns>
        Task<Participant> UpdateParticipantAsync(Participant participantEntity);
    }
}
