using Events_WEB_APP.Core.Entities;
using FluentValidation;

namespace Events_WEB_APP.Application.Validators
{
    /// <summary>
    /// Валидатор для участников.
    /// </summary>
    public class ParticipantValidator : AbstractValidator<Participant>
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ParticipantValidator"/>.
        /// </summary>
        public ParticipantValidator()
        {
            // Валидация FirstName
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(20).WithMessage("First name cannot exceed 20 characters");

            // Валидация LastName
            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(20).WithMessage("Last name cannot exceed 20 characters");

            // Валидация BirthDate
            RuleFor(p => p.BirthDate)
                .NotEmpty().WithMessage("Birth date is required")
                .Must(BeAValidAge).WithMessage("Participant must be at least 18 years old");

            // Валидация Email
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(254).WithMessage("Email cannot exceed 254 characters");

            // Валидация UserId
            RuleFor(p => p.UserId)
                .NotEmpty().WithMessage("User ID is required");
        }

        /// <summary>
        /// Проверка возраста (минимум 18 лет).
        /// </summary>
        /// <param name="birthDate">Дата рождения участника.</param>
        /// <returns>true, если участник старше 18 лет; в противном случае false.</returns>
        private bool BeAValidAge(DateTime birthDate)
        {
            var today = DateTime.UtcNow;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age >= 18;
        }
    }
}