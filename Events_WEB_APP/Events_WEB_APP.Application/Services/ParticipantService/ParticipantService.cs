using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts.Participant;
using Events_WEB_APP.Persistence.UnitsOfWork;

namespace Events_WEB_APP.Application.Services.ParticipantService
{
    public class ParticipantService : IParticipantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int MaxPageSize = 50;

        public ParticipantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Participant> CreateParticipantAsync(Participant participant)
        {
            ArgumentNullException.ThrowIfNull(participant);
            
            ValidateParticipant(participant);

            await ValidateRelationships(participant);
            await CheckDuplicateParticipation(participant);

            await _unitOfWork.Participants.AddAsync(participant);
            await _unitOfWork.CommitAsync();
            return participant;
        }

        public async Task<Participant> DeleteParticipantAsync(Guid participantId)
        {
            if (participantId == Guid.Empty)
                throw new ArgumentException("Invalid participant ID", nameof(participantId));

            var participant = await GetParticipantByIdAsync(participantId);
            await _unitOfWork.Participants.DeleteAsync(participant);
            await _unitOfWork.CommitAsync();
            return participant;
        }

        public async Task<List<Participant>> GetAllParticipantsAsync()
        {
            return (await _unitOfWork.Participants.GetAllAsync()).ToList();
        }

        public async Task<Participant> GetParticipantByIdAsync(Guid participantId)
        {
            if (participantId == Guid.Empty)
                throw new ArgumentException("Invalid participant ID", nameof(participantId));

            return await _unitOfWork.Participants.GetByIdAsync(participantId)
                ?? throw new KeyNotFoundException($"Participant with ID {participantId} not found");
        }

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

        public async Task<Participant> UpdateParticipantAsync(Participant participantEntity)
        {
            ArgumentNullException.ThrowIfNull(participantEntity);
            ValidateParticipant(participantEntity);

            var existingParticipant = await GetParticipantByIdAsync(participantEntity.Id);

            existingParticipant.FirstName = participantEntity.FirstName;
            existingParticipant.LastName = participantEntity.LastName;
            existingParticipant.BirthDate = participantEntity.BirthDate;
            existingParticipant.Email = participantEntity.Email;

            await _unitOfWork.Participants.UpdateAsync(existingParticipant);
            await _unitOfWork.CommitAsync();
            return existingParticipant;
        }

        private void ValidateParticipant(Participant participant)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(participant.FirstName) || participant.FirstName.Length > 20)
                errors.Add("First name must be between 1-20 characters");

            if (string.IsNullOrWhiteSpace(participant.LastName) || participant.LastName.Length > 20)
                errors.Add("Last name must be between 1-20 characters");

            if (participant.BirthDate > DateTime.Now.AddYears(-18))
                errors.Add("Participant must be at least 18 years old");

            if (!IsValidEmail(participant.Email))
                errors.Add("Invalid email format");

            if (errors.Any())
                throw new ArgumentException(string.Join(", ", errors));
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

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
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
