using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.UnitsOfWork;
using FluentValidation;

namespace Events_WEB_APP.Application.Services.ParticipantService
{
    /// <summary>
    /// Сервис для управления участниками.
    /// </summary>
    public class ParticipantService : IParticipantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<Participant> _validator;
        private const int MaxPageSize = 50;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParticipantService"/>.
        /// </summary>
        /// <param name="unitOfWork">Единица работы для доступа к репозиториям.</param>
        /// <param name="validator">Валидатор для проверки участников.</param>
        public ParticipantService(IUnitOfWork unitOfWork, IValidator<Participant> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        /// <summary>
        /// Создает нового участника.
        /// </summary>
        /// <param name="participant">Участник для создания.</param>
        /// <returns>Созданный участник.</returns>
        public async Task<Participant> CreateParticipantAsync(Participant participant)
        {
            ArgumentNullException.ThrowIfNull(participant);
            
            await ValidateParticipant(participant);

            await ValidateRelationships(participant);
            await CheckDuplicateParticipation(participant);

            await _unitOfWork.Participants.AddAsync(participant);
            await _unitOfWork.CommitAsync();
            return participant;
        }

        /// <summary>
        /// Удаляет участника по идентификатору.
        /// </summary>
        /// <param name="participantId">Идентификатор участника для удаления.</param>
        /// <returns>Удаленный участник.</returns>
        public async Task<Participant> DeleteParticipantAsync(Guid participantId)
        {
            if (participantId == Guid.Empty)
                throw new ArgumentException("Invalid participant ID", nameof(participantId));

            var participant = await GetParticipantByIdAsync(participantId);
            await _unitOfWork.Participants.DeleteAsync(participant);
            await _unitOfWork.CommitAsync();
            return participant;
        }

        /// <summary>
        /// Получает всех участников.
        /// </summary>
        /// <returns>Список всех участников.</returns>
        public async Task<List<Participant>> GetAllParticipantsAsync()
        {
            return (await _unitOfWork.Participants.GetAllAsync()).ToList();
        }

        /// <summary>
        /// Получает участника по идентификатору.
        /// </summary>
        /// <param name="participantId">Идентификатор участника.</param>
        /// <returns>Найденный участник.</returns>
        public async Task<Participant> GetParticipantByIdAsync(Guid participantId)
        {
            if (participantId == Guid.Empty)
                throw new ArgumentException("Invalid participant ID", nameof(participantId));

            return await _unitOfWork.Participants.GetByIdAsync(participantId)
                ?? throw new KeyNotFoundException($"Participant with ID {participantId} not found");
        }

        /// <summary>
        /// Получает участников по идентификатору события с пагинацией.
        /// </summary>
        /// <param name="eventId">Идентификатор события.</param>
        /// <param name="pageNo">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Список участников события с пагинацией.</returns>
        public async Task<List<Participant>> GetParticipantsByEventIdPaginatedAsync(
            Guid? eventId,
            int pageNo = 1,
            int pageSize = 10)
        {
            ValidatePaginationParameters(pageNo, pageSize);

            var query = (await _unitOfWork.Participants.GetAllAsync()).AsQueryable();

            if (eventId.HasValue && eventId != Guid.Empty)
            {
                query = query.Where(p => p.EventId == eventId);
            }

            return ApplyPagination(query, pageNo, pageSize).ToList();
        }

        /// <summary>
        /// Получает участников по идентификатору пользователя с пагинацией.
        /// </summary>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="pageNo">Номер страницы.</param>
        /// <param name="pageSize">Размер страницы.</param>
        /// <returns>Список участников пользователя с пагинацией.</returns>
        public async Task<List<Participant>> GetParticipantsByUserIdPaginatedAsync(
            Guid? userId,
            int pageNo = 1,
            int pageSize = 10)
        {
            ValidatePaginationParameters(pageNo, pageSize);

            var query = (await _unitOfWork.Participants.GetAllAsync()).AsQueryable();

            if (userId.HasValue && userId != Guid.Empty)
            {
                query = query.Where(p => p.UserId == userId);
            }

            return ApplyPagination(query, pageNo, pageSize).ToList();
        }

        /// <summary>
        /// Обновляет существующего участника.
        /// </summary>
        /// <param name="participantEntity">Обновленный участник.</param>
        /// <returns>Обновленный участник.</returns>
        public async Task<Participant> UpdateParticipantAsync(Participant participantEntity)
        {
            ArgumentNullException.ThrowIfNull(participantEntity);
            await ValidateParticipant(participantEntity);

            var existingParticipant = await GetParticipantByIdAsync(participantEntity.Id);

            existingParticipant.FirstName = participantEntity.FirstName;
            existingParticipant.LastName = participantEntity.LastName;
            existingParticipant.BirthDate = participantEntity.BirthDate;
            existingParticipant.Email = participantEntity.Email;

            await _unitOfWork.Participants.UpdateAsync(existingParticipant);
            await _unitOfWork.CommitAsync();
            return existingParticipant;
        }

        private async Task ValidateParticipant(Participant participant)
        {
            var validationResult = await _validator.ValidateAsync(participant);

            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ArgumentException(errors);
            }
        }

        private async Task ValidateRelationships(Participant participant)
        {
            if (await _unitOfWork.Users.GetByIdAsync(participant.UserId) == null)
                throw new ArgumentException("User not found", nameof(participant.UserId));

            if (await _unitOfWork.Events.GetByIdAsync(participant.EventId) == null)
                throw new ArgumentException("Event not found", nameof(participant.EventId));
        }

        private async Task CheckDuplicateParticipation(Participant participant)
        {
            var existingParticipant = (await _unitOfWork.Participants
                .GetAllAsync())
                .Where(p => p.UserId == participant.UserId &&
                              p.EventId == participant.EventId);

            if (existingParticipant.Any())
                throw new InvalidOperationException("User is already registered for this event");
        }

        private void ValidatePaginationParameters(int pageNo, int pageSize)
        {
            if (pageNo < 1) throw new ArgumentException("Page number must be ≥ 1", nameof(pageNo));
            if (pageSize < 1 || pageSize > MaxPageSize)
                throw new ArgumentException($"Page size must be between 1-{MaxPageSize}", nameof(pageSize));
        }

        private IQueryable<Participant> ApplyPagination(IQueryable<Participant> query, int pageNo, int pageSize)
        {
            return query
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
